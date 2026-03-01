using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;
using WarehouseManager.Domain.Events;
using WarehouseManager.Domain.States;

namespace WarehouseManager.Domain.Entities;

public class Order : BaseEntity, IPrototype<Order>
{
    public string OrderNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string? ShippingAddress { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    private IOrderState CurrentState => OrderStateFactory.Create(Status);

    public Order Clone()
    {
        var clone = new Order
        {
            CustomerId = CustomerId,
            ShippingAddress = ShippingAddress,
            TotalAmount = TotalAmount
        };

        foreach (var item in Items)
            clone.Items.Add(item.Clone());

        return clone;
    }

    public Order CloneAsNewOrder()
    {
        var clone = Clone();
        clone.OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        clone.CalculateTotal();
        return clone;
    }

    public Result Confirm()
    {
        return ApplyTransition(state => state.Confirm());
    }

    public Result StartProcessing()
    {
        return ApplyTransition(state => state.StartProcessing());
    }

    public Result Ship()
    {
        return ApplyTransition(state => state.Ship());
    }

    public Result Deliver()
    {
        return ApplyTransition(state => state.Deliver());
    }

    public Result Cancel()
    {
        return ApplyTransition(state => state.Cancel());
    }

    private Result ApplyTransition(Func<IOrderState, Result<OrderStatus>> transition)
    {
        var result = transition(CurrentState);
        if (result.IsFailure)
            return Result.Failure(result.Error!);

        var oldStatus = Status.ToString();
        Status = result.Value;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(Id, oldStatus, Status.ToString()));

        return Result.Success();
    }

    public void CalculateTotal()
    {
        TotalAmount = Items.Sum(i => i.Quantity * i.UnitPrice);
    }

    public void MarkAsCreated()
    {
        AddDomainEvent(new OrderCreatedEvent(Id, CustomerId, TotalAmount));
    }
}