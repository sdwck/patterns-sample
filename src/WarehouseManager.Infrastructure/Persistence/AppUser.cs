using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Infrastructure.Persistence;

public class AppUser : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    public Guid? CustomerId { get; set; }
}