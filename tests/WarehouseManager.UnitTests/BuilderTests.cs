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
}