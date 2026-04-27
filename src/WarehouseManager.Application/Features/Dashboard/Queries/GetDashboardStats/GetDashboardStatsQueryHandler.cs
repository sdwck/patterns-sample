using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Dashboard.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IUnitOfWork _uow;

    public GetDashboardStatsQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken ct)
    {
        var orders = await _uow.Orders.GetAllAsync(ct);
        var customers = await _uow.Customers.GetAllAsync(ct);
        var lowStock = await _uow.Stock.GetLowStockAsync(ct);

        var totalOrders = orders.Count;
        var totalRevenue = orders.Sum(o => o.TotalAmount);
        var totalCustomers = customers.Count;
        var lowStockCount = lowStock.Count;

        return new DashboardStatsDto(totalOrders, totalRevenue, lowStockCount, totalCustomers);
    }
}