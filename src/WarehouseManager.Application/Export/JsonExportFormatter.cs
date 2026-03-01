using System.Text.Json;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Export;

public class JsonExportFormatter : IExportFormatter
{
    public string FileExtension => "json";
    public string ContentType => "application/json";

    public string Format(StockReport report)
    {
        return JsonSerializer.Serialize(report,
            new JsonSerializerOptions { WriteIndented = true });
    }
}