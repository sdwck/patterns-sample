using MediatR;
using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Features.Dashboard.Queries.GetDashboardStats;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;