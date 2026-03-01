using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class CancelledState : IOrderState
{
    public string Name => "Cancelled";

    public Result<OrderStatus> Confirm()
    {
        return Result.Failure<OrderStatus>("Order is cancelled.");
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Failure<OrderStatus>("Order is cancelled.");
    }

    public Result<OrderStatus> Ship()
    {
        return Result.Failure<OrderStatus>("Order is cancelled.");
    }

    public Result<OrderStatus> Deliver()
    {
        return Result.Failure<OrderStatus>("Order is cancelled.");
    }

    public Result<OrderStatus> Cancel()
    {
        return Result.Failure<OrderStatus>("Order is already cancelled.");
    }
}