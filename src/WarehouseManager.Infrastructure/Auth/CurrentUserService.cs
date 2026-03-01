using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? UserId => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? Role => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}