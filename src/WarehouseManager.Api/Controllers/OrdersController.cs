using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Features.Orders.Commands.Reorder;
using WarehouseManager.Application.Features.Orders.Commands.UpdateOrderStatus;
using WarehouseManager.Application.Features.Orders.Queries.GetAllOrders;
using WarehouseManager.Application.Features.Orders.Queries.ResolveCustomerId;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Api.Controllers;

public record PlaceOrderRequest(
    Guid? CustomerId,
    string? ShippingAddress,
    List<OrderItemRequest> Items,
    string? DiscountStrategy);

public record UpdateStatusRequest(string Action);

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly Facade _facade;
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator, Facade facade)
    {
        _mediator = mediator;
        _facade = facade;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isPrivileged = role is "Admin" or "Manager";

        Guid? filterByCustomerId = null;

        if (!isPrivileged)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var customerIdResult = await _mediator.Send(new ResolveCustomerIdQuery(userId));
            if (customerIdResult.IsFailure)
                return BadRequest(new { error = customerIdResult.Error });

            filterByCustomerId = customerIdResult.Value;
        }

        var result = await _mediator.Send(new GetAllOrdersQuery(page, pageSize, filterByCustomerId));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _facade.GetOrderDetailsAsync(id);
        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isPrivileged = role is "Admin" or "Manager";

        if (!isPrivileged)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var customerIdResult = await _mediator.Send(new ResolveCustomerIdQuery(userId));
            if (customerIdResult.IsFailure || customerIdResult.Value != result.Value!.CustomerId)
                return Forbid();
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] PlaceOrderRequest request)
    {
        var customerId = await ResolveCustomerIdAsync(request.CustomerId);
        if (customerId is null)
            return BadRequest(new
                { error = "Could not resolve customer. Ensure your account has a linked customer profile." });

        var result = await _facade.PlaceOrderAsync(
            customerId.Value,
            request.ShippingAddress,
            request.Items,
            request.DiscountStrategy ?? "None");

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpPost("{id:guid}/reorder")]
    [Authorize]
    public async Task<IActionResult> Reorder(Guid id)
    {
        var result = await _mediator.Send(new ReorderCommand(id));
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value })
            : BadRequest(new { error = result.Error });
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateOrderStatusCommand(id, request.Action));
        return result.IsSuccess ? Ok() : BadRequest(new { error = result.Error });
    }

    private async Task<Guid?> ResolveCustomerIdAsync(Guid? explicitCustomerId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var isPrivileged = role is "Admin" or "Manager";

        if (explicitCustomerId.HasValue && isPrivileged)
            return explicitCustomerId.Value;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return null;

        var result = await _mediator.Send(new ResolveCustomerIdQuery(userId));
        return result.IsSuccess ? result.Value : null;
    }
}