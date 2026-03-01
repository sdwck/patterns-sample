namespace WarehouseManager.Application.DTOs;

public record OrderDto(
    Guid Id,
    string OrderNumber,
    Guid CustomerId,
    string CustomerName,
    string Status,
    decimal TotalAmount,
    string? ShippingAddress,
    DateTime CreatedAt,
    List<OrderItemDto> Items);

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Total);