using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public string FullName => $"{FirstName} {LastName}";
}