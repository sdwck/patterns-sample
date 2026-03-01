using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class DeliveredState : IOrderState
{
    public string Name => "Delivered";

    public Result<OrderStatus> Confirm()
    {
        return Result.Failure<OrderStatus>("Order is already delivered.");
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Failure<OrderStatus>("Order is already delivered.");
    }

    public Result<OrderStatus> Ship()
    {
        return Result.Failure<OrderStatus>("Order is already delivered.");
    }

    public Result<OrderStatus> Deliver()
    {
        return Result.Failure<OrderStatus>("Order is already delivered.");
    }

    public Result<OrderStatus> Cancel()
    {
        return Result.Failure<OrderStatus>("Cannot cancel delivered order.");
    }
}