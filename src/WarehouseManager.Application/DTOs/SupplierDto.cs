namespace WarehouseManager.Application.DTOs;

public record SupplierDto(
    Guid Id,
    string Name,
    string? ContactEmail,
    string? Phone,
    string? Address);