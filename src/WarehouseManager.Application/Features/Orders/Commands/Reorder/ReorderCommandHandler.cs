using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Orders.Commands.Reorder;

public class ReorderCommandHandler : IRequestHandler<ReorderCommand, Result<Guid>>
{
    private readonly IUnitOfWork _uow;

    public ReorderCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<Guid>> Handle(ReorderCommand request, CancellationToken ct)
    {
        var originalOrder = await _uow.Orders.GetWithItemsAsync(request.OriginalOrderId, ct);
        if (originalOrder is null)
            return Result.Failure<Guid>("Original order not found.");

        var newOrder = originalOrder.CloneAsNewOrder();

        foreach (var item in newOrder.Items)
        {
            var product = await _uow.Products.GetByIdAsync(item.ProductId, ct);
            if (product is null)
                return Result.Failure<Guid>($"Product {item.ProductId} no longer exists.");

            var stock = await _uow.Stock.GetByProductIdAsync(item.ProductId, ct);
            if (stock is null)
                return Result.Failure<Guid>($"Stock record for product '{product.Name}' not found.");

            item.UnitPrice = product.Price;

            var deductResult = stock.Deduct(item.Quantity);
            if (deductResult.IsFailure)
                return Result.Failure<Guid>($"Cannot reorder: {deductResult.Error}");
        }

        newOrder.CalculateTotal();
        newOrder.MarkAsCreated();
        await _uow.Orders.AddAsync(newOrder, ct);
        await _uow.SaveChangesAsync(ct);

        return Result.Success(newOrder.Id);
    }
}