using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Features.Products.Commands.CreateProduct;
using WarehouseManager.Application.Features.Products.Commands.DeleteProduct;
using WarehouseManager.Application.Features.Products.Commands.UpdateProduct;
using WarehouseManager.Application.Features.Products.Queries.GetAllProducts;
using WarehouseManager.Application.Features.Products.Queries.GetProductById;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null, [FromQuery] Guid? categoryId = null,
        [FromQuery] string? sortBy = null, [FromQuery] bool sortDescending = false)
    {
        return Ok(await _mediator.Send(new GetAllProductsQuery(page, pageSize, search, categoryId, sortBy, sortDescending)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var r = await _mediator.Send(new GetProductByIdQuery(id));
        return r.IsSuccess ? Ok(r.Value) : NotFound(new { error = r.Error });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
    {
        var r = await _mediator.Send(cmd);
        return r.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = r.Value }, new { id = r.Value })
            : BadRequest(new { error = r.Error });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand cmd)
    {
        if (id != cmd.Id) return BadRequest(new { error = "ID mismatch." });
        var r = await _mediator.Send(cmd);
        return r.IsSuccess ? NoContent() : NotFound(new { error = r.Error });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var r = await _mediator.Send(new DeleteProductCommand(id));
        return r.IsSuccess ? NoContent() : NotFound(new { error = r.Error });
    }
}