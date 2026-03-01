using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public Stock? Stock { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}