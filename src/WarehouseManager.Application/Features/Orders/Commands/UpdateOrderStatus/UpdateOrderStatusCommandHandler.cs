using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateOrderStatusCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        var order = await _uow.Orders.GetWithItemsAsync(request.OrderId, ct);
        if (order is null) return Result.Failure("Order not found.");

        var result = request.Action.ToLower() switch
        {
            "confirm" => order.Confirm(),
            "process" => order.StartProcessing(),
            "ship" => order.Ship(),
            "deliver" => order.Deliver(),
            "cancel" => order.Cancel(),
            _ => Result.Failure($"Unknown action: {request.Action}")
        };

        if (result.IsFailure) return result;

        _uow.Orders.Update(order);
        await _uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}