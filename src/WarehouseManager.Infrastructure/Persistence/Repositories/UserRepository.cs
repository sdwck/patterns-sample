using Microsoft.EntityFrameworkCore;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        return user is null ? null : MapToDto(user);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<Guid> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var user = new AppUser
        {
            Email = request.Email,
            PasswordHash = request.PasswordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = Enum.Parse<UserRole>(request.Role),
            CustomerId = request.CustomerId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

        return user.Id;
    }

    private static UserDto MapToDto(AppUser user)
    {
        return new UserDto(user.Id, user.Email, user.FirstName, user.LastName, user.Role.ToString(), user.CustomerId);
    }
}