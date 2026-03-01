using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class PendingState : IOrderState
{
    public string Name => "Pending";

    public Result<OrderStatus> Confirm()
    {
        return Result.Success(OrderStatus.Confirmed);
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Failure<OrderStatus>("Order must be confirmed before processing.");
    }

    public Result<OrderStatus> Ship()
    {
        return Result.Failure<OrderStatus>("Order must be processed before shipping.");
    }

    public Result<OrderStatus> Deliver()
    {
        return Result.Failure<OrderStatus>("Order must be shipped before delivery.");
    }

    public Result<OrderStatus> Cancel()
    {
        return Result.Success(OrderStatus.Cancelled);
    }
}