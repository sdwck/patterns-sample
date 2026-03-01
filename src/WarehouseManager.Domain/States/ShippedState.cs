using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public Result<OrderStatus> Confirm()
    {
        return Result.Failure<OrderStatus>("Order is already past confirmation.");
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Failure<OrderStatus>("Order is already shipped.");
    }

    public Result<OrderStatus> Ship()
    {
        return Result.Failure<OrderStatus>("Order is already shipped.");
    }

    public Result<OrderStatus> Deliver()
    {
        return Result.Success(OrderStatus.Delivered);
    }

    public Result<OrderStatus> Cancel()
    {
        return Result.Failure<OrderStatus>("Cannot cancel shipped order.");
    }
}