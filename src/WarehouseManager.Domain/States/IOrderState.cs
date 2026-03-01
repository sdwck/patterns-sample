using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Enums;

namespace WarehouseManager.Domain.States;

public interface IOrderState
{
    string Name { get; }
    Result<OrderStatus> Confirm();
    Result<OrderStatus> StartProcessing();
    Result<OrderStatus> Ship();
    Result<OrderStatus> Deliver();
    Result<OrderStatus> Cancel();
}