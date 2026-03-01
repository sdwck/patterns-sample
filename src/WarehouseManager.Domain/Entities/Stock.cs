using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;

namespace WarehouseManager.Domain.Entities;

public class Stock : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int QuantityOnHand { get; set; }
    public int ReorderLevel { get; set; } = 10;
    public string? WarehouseLocation { get; set; }

    public Result Deduct(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure("Quantity must be positive.");
        if (QuantityOnHand < quantity)
            return Result.Failure($"Insufficient stock. Available: {QuantityOnHand}, Requested: {quantity}");

        QuantityOnHand -= quantity;
        UpdatedAt = DateTime.UtcNow;

        if (QuantityOnHand <= ReorderLevel)
            AddDomainEvent(new LowStockEvent(ProductId, QuantityOnHand, ReorderLevel));

        return Result.Success();
    }

    public void Restock(int quantity)
    {
        QuantityOnHand += quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}