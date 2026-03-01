using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    private readonly IUnitOfWork _uow;

    public GetProductByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var p = await _uow.Products.GetByIdAsync(request.Id, ct);
        if (p is null) return Result.Failure<ProductDto>("Product not found.");

        return Result.Success(new ProductDto(
            p.Id, p.Name, p.Description, p.Sku, p.Price,
            p.CategoryId, p.Category.Name, p.SupplierId,
            p.Stock?.QuantityOnHand ?? 0));
    }
}