using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Features.Suppliers.Commands.CreateSupplier;
using WarehouseManager.Application.Features.Suppliers.Queries.GetAllSuppliers;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllSuppliersQuery()));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateSupplierCommand cmd)
    {
        var r = await _mediator.Send(cmd);
        return r.IsSuccess ? Ok(new { id = r.Value }) : BadRequest(new { error = r.Error });
    }
}