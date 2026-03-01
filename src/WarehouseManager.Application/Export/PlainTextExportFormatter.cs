using System.Text;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Export;

public class PlainTextExportFormatter : IExportFormatter
{
    public string FileExtension => "txt";
    public string ContentType => "text/plain";

    public string Format(StockReport report)
    {
        var sb = new StringBuilder();
        sb.AppendLine(report.Title);
        sb.AppendLine(new string('=', report.Title.Length));
        sb.AppendLine();

        foreach (var item in report.Items)
        {
            var status = item.IsLowStock ? "[LOW]" : "[ OK]";
            sb.AppendLine(
                $"  {status} {item.ProductName} (SKU: {item.Sku}) — Qty: {item.Quantity}, Location: {item.Location ?? "N/A"}");
        }

        sb.AppendLine();
        sb.AppendLine(report.Summary);
        return sb.ToString();
    }
}