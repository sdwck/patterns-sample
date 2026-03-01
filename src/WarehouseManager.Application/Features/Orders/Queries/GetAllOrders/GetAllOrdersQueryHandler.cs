using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllOrdersQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken ct)
    {
        var all = await _uow.Orders.GetAllAsync(ct);

        var query = all.ToList();

        if (request.FilterByCustomerId.HasValue)
            query = query.Where(o => o.CustomerId == request.FilterByCustomerId.Value).ToList();

        var totalCount = query.Count;

        var items = query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(o => new OrderDto(
                o.Id, o.OrderNumber, o.CustomerId,
                o.Customer?.FullName ?? "", o.Status.ToString(),
                o.TotalAmount, o.ShippingAddress, o.CreatedAt,
                o.Items.Select(i => new OrderItemDto(
                    i.Id, i.ProductId, i.Product?.Name ?? "",
                    i.Quantity, i.UnitPrice, i.Total)).ToList()))
            .ToList();

        return new PagedResult<OrderDto>(items, totalCount, request.Page, request.PageSize);
    }
}