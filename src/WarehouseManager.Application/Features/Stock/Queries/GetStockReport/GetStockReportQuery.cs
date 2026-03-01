using MediatR;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Application.Features.Stock.Queries.GetStockReport;

public record GetStockReportQuery : IRequest<StockReport>;