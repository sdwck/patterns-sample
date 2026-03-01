using System.Text;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Export;

public class CsvExportFormatter : IExportFormatter
{
    public string FileExtension => "csv";
    public string ContentType => "text/csv";

    public string Format(StockReport report)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ProductName,SKU,Quantity,Location,IsLowStock");
        foreach (var item in report.Items)
        {
            var location = item.Location?.Replace(",", ";") ?? "";
            sb.AppendLine($"\"{item.ProductName}\",\"{item.Sku}\",{item.Quantity},\"{location}\",{item.IsLowStock}");
        }

        return sb.ToString();
    }
}