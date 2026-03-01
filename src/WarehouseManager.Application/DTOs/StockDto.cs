namespace WarehouseManager.Application.DTOs;

public record StockDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int QuantityOnHand,
    int ReorderLevel,
    string? WarehouseLocation);