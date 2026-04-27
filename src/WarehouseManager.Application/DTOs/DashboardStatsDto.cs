namespace WarehouseManager.Application.DTOs;

public record DashboardStatsDto(
    int TotalOrders,
    decimal TotalRevenue,
    int LowStockItemsCount,
    int TotalCustomers
);