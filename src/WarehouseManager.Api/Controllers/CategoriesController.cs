using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Features.Categories.Commands.CreateCategory;
using WarehouseManager.Application.Features.Categories.Queries.GetAllCategories;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllCategoriesQuery()));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand cmd)
    {
        var r = await _mediator.Send(cmd);
        return r.IsSuccess ? Ok(new { id = r.Value }) : BadRequest(new { error = r.Error });
    }
}