using MediatR;
using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Features.Stock.Queries.GetLowStock;

public record GetLowStockQuery : IRequest<List<StockDto>>;