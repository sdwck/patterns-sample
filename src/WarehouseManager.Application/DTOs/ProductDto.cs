namespace WarehouseManager.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    Guid CategoryId,
    string CategoryName,
    Guid? SupplierId,
    int StockQuantity);