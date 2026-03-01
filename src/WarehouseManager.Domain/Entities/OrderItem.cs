using WarehouseManager.Domain.Common;

namespace WarehouseManager.Domain.Entities;

public class OrderItem : BaseEntity, IPrototype<OrderItem>
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Quantity * UnitPrice;

    public OrderItem Clone()
    {
        return new OrderItem
        {
            ProductId = ProductId,
            Quantity = Quantity,
            UnitPrice = UnitPrice
        };
    }
}