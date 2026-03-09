using FluentAssertions;
using WarehouseManager.Application.Services;

namespace WarehouseManager.UnitTests;

public class BuilderTests
{
    [Fact]
    public void Build_SetsDefaults()
    {
        var report = new StockReportBuilder().Build();
        report.Title.Should().Be("Stock Report");
        report.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void LowStockOnly_FiltersItems()
    {
        var report = new StockReportBuilder()
            .AddItem(new StockReportItem { ProductName = "A", IsLowStock = true })
            .AddItem(new StockReportItem { ProductName = "B", IsLowStock = false })
            .LowStockOnly()
            .Build();

        report.Items.Should().HaveCount(1);
        report.Items[0].ProductName.Should().Be("A");
    }

    [Fact]
    public void Director_BuildFullStockReport_SetsTitle()
    {
        var items = new List<StockReportItem>
        {
            new() { ProductName = "X", Quantity = 10, IsLowStock = false },
            new() { ProductName = "Y", Quantity = 2, IsLowStock = true }
        };

        var director = new StockReportDirector(new StockReportBuilder());
        var report = director.BuildFullStockReport(items);

        report.Title.Should().Be("Warehouse Stock Report");
        report.Items.Should().HaveCount(2);
        report.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Director_BuildLowStockReport_FiltersItems()
    {
        var items = new List<StockReportItem>
        {
            new() { ProductName = "X", Quantity = 10, IsLowStock = false },
            new() { ProductName = "Y", Quantity = 2, IsLowStock = true }
        };

        var director = new StockReportDirector(new StockReportBuilder());
        var report = director.BuildLowStockReport(items);

        report.Title.Should().Be("Low Stock Alert Report");
        report.Items.Should().HaveCount(1);
        report.Items[0].ProductName.Should().Be("Y");
    }
}