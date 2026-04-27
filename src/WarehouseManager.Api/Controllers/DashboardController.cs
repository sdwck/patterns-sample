using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.Features.Dashboard.Queries.GetDashboardStats;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("stats")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetStats()
    {
        return Ok(await _mediator.Send(new GetDashboardStatsQuery()));
    }
}