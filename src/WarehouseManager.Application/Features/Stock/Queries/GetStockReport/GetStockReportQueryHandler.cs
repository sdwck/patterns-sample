using MediatR;
using WarehouseManager.Application.Services;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Stock.Queries.GetStockReport;

public class GetStockReportQueryHandler : IRequestHandler<GetStockReportQuery, StockReport>
{
    private readonly IUnitOfWork _uow;

    public GetStockReportQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<StockReport> Handle(GetStockReportQuery request, CancellationToken ct)
    {
        var all = await _uow.Stock.GetAllAsync(ct);

        var report = new StockReportBuilder()
            .WithTitle("Warehouse Stock Report")
            .GeneratedAt(DateTime.UtcNow)
            .AddItems(all.Select(s => new StockReportItem
            {
                ProductName = s.Product?.Name ?? "Unknown",
                Sku = s.Product?.Sku ?? "",
                Quantity = s.QuantityOnHand,
                Location = s.WarehouseLocation,
                IsLowStock = s.QuantityOnHand <= s.ReorderLevel
            }))
            .Build();

        return report;
    }
}