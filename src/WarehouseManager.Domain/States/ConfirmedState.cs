using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class ConfirmedState : IOrderState
{
    public string Name => "Confirmed";

    public Result<OrderStatus> Confirm()
    {
        return Result.Failure<OrderStatus>("Order is already confirmed.");
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Success(OrderStatus.Processing);
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