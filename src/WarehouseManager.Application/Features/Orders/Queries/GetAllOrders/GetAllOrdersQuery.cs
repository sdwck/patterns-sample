using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? FilterByCustomerId = null) : IRequest<PagedResult<OrderDto>>;