using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Stock.Queries.GetLowStock;

public class GetLowStockQueryHandler : IRequestHandler<GetLowStockQuery, List<StockDto>>
{
    private readonly IUnitOfWork _uow;

    public GetLowStockQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<StockDto>> Handle(GetLowStockQuery request, CancellationToken ct)
    {
        var low = await _uow.Stock.GetLowStockAsync(ct);
        return low.Select(s => new StockDto(
            s.Id, s.ProductId, s.Product?.Name ?? "",
            s.QuantityOnHand, s.ReorderLevel, s.WarehouseLocation)).ToList();
    }
}