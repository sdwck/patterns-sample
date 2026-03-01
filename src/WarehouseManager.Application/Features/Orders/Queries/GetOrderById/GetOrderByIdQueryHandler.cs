using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IUnitOfWork _uow;

    public GetOrderByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var o = await _uow.Orders.GetWithItemsAsync(request.Id, ct);
        if (o is null) return Result.Failure<OrderDto>("Order not found.");

        return Result.Success(new OrderDto(
            o.Id, o.OrderNumber, o.CustomerId, o.Customer?.FullName ?? "",
            o.Status.ToString(), o.TotalAmount, o.ShippingAddress, o.CreatedAt,
            o.Items.Select(i => new OrderItemDto(
                i.Id, i.ProductId, i.Product?.Name ?? "", i.Quantity, i.UnitPrice, i.Total)).ToList()));
    }
}