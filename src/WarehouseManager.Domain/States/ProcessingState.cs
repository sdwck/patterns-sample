using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public class ProcessingState : IOrderState
{
    public string Name => "Processing";

    public Result<OrderStatus> Confirm()
    {
        return Result.Failure<OrderStatus>("Order is already past confirmation.");
    }

    public Result<OrderStatus> StartProcessing()
    {
        return Result.Failure<OrderStatus>("Order is already being processed.");
    }

    public Result<OrderStatus> Ship()
    {
        return Result.Success(OrderStatus.Shipped);
    }

    public Result<OrderStatus> Deliver()
    {
        return Result.Failure<OrderStatus>("Order must be shipped before delivery.");
    }

    public Result<OrderStatus> Cancel()
    {
        return Result.Failure<OrderStatus>("Cannot cancel order that is being processed.");
    }
}