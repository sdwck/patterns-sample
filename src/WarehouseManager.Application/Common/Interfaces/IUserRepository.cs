using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(string email, CancellationToken ct = default);
    Task<bool> VerifyPasswordAsync(string email, string password, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
}