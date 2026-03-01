using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Export;

public record ExportedReport(string Content, string ContentType, string FileName);

public abstract class ReportExporter
{
    protected abstract IExportFormatter CreateFormatter();

    public ExportedReport Export(StockReport report)
    {
        var formatter = CreateFormatter();
        var content = formatter.Format(report);
        var fileName = GenerateFileName(report, formatter.FileExtension);
        return new ExportedReport(content, formatter.ContentType, fileName);
    }

    private static string GenerateFileName(StockReport report, string extension)
    {
        var safeName = report.Title.Replace(" ", "_");
        return $"{safeName}_{report.GeneratedAt:yyyyMMdd_HHmmss}.{extension}";
    }
}

public class JsonReportExporter : ReportExporter
{
    protected override IExportFormatter CreateFormatter()
    {
        return new JsonExportFormatter();
    }
}

public class CsvReportExporter : ReportExporter
{
    protected override IExportFormatter CreateFormatter()
    {
        return new CsvExportFormatter();
    }
}

public class PlainTextReportExporter : ReportExporter
{
    protected override IExportFormatter CreateFormatter()
    {
        return new PlainTextExportFormatter();
    }
}

public static class ReportExporterFactory
{
    public static ReportExporter Create(string format)
    {
        return format.ToLower() switch
        {
            "csv" => new CsvReportExporter(),
            "text" or "txt" => new PlainTextReportExporter(),
            _ => new JsonReportExporter()
        };
    }
}