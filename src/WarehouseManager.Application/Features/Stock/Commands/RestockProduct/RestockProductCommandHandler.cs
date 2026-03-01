using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Stock.Commands.RestockProduct;

public class RestockProductCommandHandler : IRequestHandler<RestockProductCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public RestockProductCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(RestockProductCommand request, CancellationToken ct)
    {
        var stock = await _uow.Stock.GetByProductIdAsync(request.ProductId, ct);
        if (stock is null) return Result.Failure("Stock not found.");
        stock.Restock(request.Quantity);
        _uow.Stock.Update(stock);
        await _uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}