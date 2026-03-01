using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Export;

public interface IExportFormatter
{
    string FileExtension { get; }
    string ContentType { get; }
    string Format(StockReport report);
}