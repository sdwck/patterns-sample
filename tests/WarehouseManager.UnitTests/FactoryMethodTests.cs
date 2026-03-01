using FluentAssertions;
using WarehouseManager.Application.Export;
using WarehouseManager.Application.Services;

namespace WarehouseManager.UnitTests;

public class FactoryMethodTests
{
    private StockReport CreateSampleReport()
    {
        return new StockReportBuilder()
            .WithTitle("Test Report")
            .GeneratedAt(new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc))
            .AddItem(new StockReportItem
            {
                ProductName = "Widget",
                Sku = "WDG-001",
                Quantity = 50,
                Location = "A1-01",
                IsLowStock = false
            })
            .AddItem(new StockReportItem
            {
                ProductName = "Gadget",
                Sku = "GDG-001",
                Quantity = 3,
                Location = "B2-05",
                IsLowStock = true
            })
            .Build();
    }

    [Fact]
    public void JsonExporter_ShouldCreateJsonFormatter()
    {
        var exporter = new JsonReportExporter();

        var result = exporter.Export(CreateSampleReport());

        result.ContentType.Should().Be("application/json");
        result.FileName.Should().EndWith(".json");
        result.Content.Should().Contain("Widget");
        result.Content.Should().Contain("WDG-001");
    }

    [Fact]
    public void CsvExporter_ShouldCreateCsvFormatter()
    {
        var exporter = new CsvReportExporter();

        var result = exporter.Export(CreateSampleReport());

        result.ContentType.Should().Be("text/csv");
        result.FileName.Should().EndWith(".csv");
        result.Content.Should().Contain("ProductName,SKU,Quantity");
        result.Content.Should().Contain("Widget");
    }

    [Fact]
    public void PlainTextExporter_ShouldCreateTextFormatter()
    {
        var exporter = new PlainTextReportExporter();

        var result = exporter.Export(CreateSampleReport());

        result.ContentType.Should().Be("text/plain");
        result.FileName.Should().EndWith(".txt");
        result.Content.Should().Contain("[LOW]");
        result.Content.Should().Contain("[ OK]");
    }

    [Fact]
    public void ReportExporterFactory_ShouldCreateCorrectExporter()
    {
        ReportExporterFactory.Create("json").Should().BeOfType<JsonReportExporter>();
        ReportExporterFactory.Create("csv").Should().BeOfType<CsvReportExporter>();
        ReportExporterFactory.Create("text").Should().BeOfType<PlainTextReportExporter>();
        ReportExporterFactory.Create("txt").Should().BeOfType<PlainTextReportExporter>();
        ReportExporterFactory.Create("unknown").Should().BeOfType<JsonReportExporter>();
    }

    [Fact]
    public void AllExporters_ShouldGenerateCorrectFileName()
    {
        var report = CreateSampleReport();

        var json = new JsonReportExporter().Export(report);
        var csv = new CsvReportExporter().Export(report);
        var txt = new PlainTextReportExporter().Export(report);

        json.FileName.Should().Contain("Test_Report");
        csv.FileName.Should().Contain("Test_Report");
        txt.FileName.Should().Contain("Test_Report");
    }

    [Fact]
    public void CsvExporter_ShouldEscapeCommasInData()
    {
        var report = new StockReportBuilder()
            .WithTitle("Test")
            .AddItem(new StockReportItem
            {
                ProductName = "Widget, Deluxe",
                Sku = "WDG-002",
                Quantity = 10,
                Location = "A1, Shelf 3",
                IsLowStock = false
            })
            .Build();

        var exporter = new CsvReportExporter();
        var result = exporter.Export(report);

        result.Content.Should().Contain("\"Widget, Deluxe\"");
        result.Content.Should().Contain("\"A1; Shelf 3\"");
    }
}