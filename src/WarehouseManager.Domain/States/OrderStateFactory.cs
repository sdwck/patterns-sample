using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public static class OrderStateFactory
{
    public static IOrderState Create(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => new PendingState(),
            OrderStatus.Confirmed => new ConfirmedState(),
            OrderStatus.Processing => new ProcessingState(),
            OrderStatus.Shipped => new ShippedState(),
            OrderStatus.Delivered => new DeliveredState(),
            OrderStatus.Cancelled => new CancelledState(),
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
}