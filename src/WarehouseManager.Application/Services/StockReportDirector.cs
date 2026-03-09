namespace WarehouseManager.Application.Services;

public class StockReportDirector
{
    private readonly IReportBuilder<StockReport, StockReportItem> _builder;

    public StockReportDirector(IReportBuilder<StockReport, StockReportItem> builder)
    {
        _builder = builder;
    }

    public StockReport BuildFullStockReport(IEnumerable<StockReportItem> items)
    {
        return _builder
            .WithTitle("Warehouse Stock Report")
            .GeneratedAt(DateTime.UtcNow)
            .AddItems(items)
            .Build();
    }

    public StockReport BuildLowStockReport(IEnumerable<StockReportItem> items)
    {
        return _builder
            .WithTitle("Low Stock Alert Report")
            .GeneratedAt(DateTime.UtcNow)
            .AddItems(items)
            .LowStockOnly()
            .Build();
    }
}