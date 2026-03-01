using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Application.Features.Auth.Commands.Login;
using WarehouseManager.Application.Features.Auth.Commands.Register;
using WarehouseManager.Application.Features.Auth.Queries.GetCurrentUser;

namespace WarehouseManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        return result.IsSuccess ? Ok(result.Value) : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result =
            await _mediator.Send(new RegisterCommand(request.Email, request.Password, request.FirstName,
                request.LastName));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetCurrentUserQuery(userId));
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }
}