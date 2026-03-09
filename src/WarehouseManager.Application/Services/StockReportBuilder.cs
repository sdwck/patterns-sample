namespace WarehouseManager.Application.Services;

public class StockReport
{
    public string Title { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public List<StockReportItem> Items { get; set; } = [];
    public string? Summary { get; set; }
    public bool IncludeLowStockOnly { get; set; }
}

public interface IReportItem
{
    bool IsLowStock { get; }
}

public interface IReportBuilder<TReport, TItem> where TItem : IReportItem
{
    IReportBuilder<TReport, TItem> WithTitle(string title);
    IReportBuilder<TReport, TItem> GeneratedAt(DateTime dt);
    IReportBuilder<TReport, TItem> AddItem(TItem item);
    IReportBuilder<TReport, TItem> AddItems(IEnumerable<TItem> items);
    IReportBuilder<TReport, TItem> LowStockOnly();
    IReportBuilder<TReport, TItem> WithSummary(string summary);
    TReport Build();
}

public class StockReportItem : IReportItem
{
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public bool IsLowStock { get; set; }
}

public class StockReportBuilder : IReportBuilder<StockReport, StockReportItem>
{
    private readonly StockReport _report = new();

    public IReportBuilder<StockReport, StockReportItem> WithTitle(string title)
    {
        _report.Title = title;
        return this;
    }

    public IReportBuilder<StockReport, StockReportItem> GeneratedAt(DateTime dt)
    {
        _report.GeneratedAt = dt;
        return this;
    }

    public IReportBuilder<StockReport, StockReportItem> AddItem(StockReportItem item)
    {
        _report.Items.Add(item);
        return this;
    }

    public IReportBuilder<StockReport, StockReportItem> AddItems(IEnumerable<StockReportItem> items)
    {
        _report.Items.AddRange(items);
        return this;
    }

    public IReportBuilder<StockReport, StockReportItem> LowStockOnly()
    {
        _report.IncludeLowStockOnly = true;
        return this;
    }

    public IReportBuilder<StockReport, StockReportItem> WithSummary(string summary)
    {
        _report.Summary = summary;
        return this;
    }

    public StockReport Build()
    {
        if (string.IsNullOrWhiteSpace(_report.Title))
            _report.Title = "Stock Report";

        if (_report.GeneratedAt == default)
            _report.GeneratedAt = DateTime.UtcNow;

        if (_report.IncludeLowStockOnly)
            _report.Items = _report.Items.Where(i => i.IsLowStock).ToList();

        _report.Summary ??= $"Total items: {_report.Items.Count}, Low stock: {_report.Items.Count(i => i.IsLowStock)}";

        return _report;
    }
}