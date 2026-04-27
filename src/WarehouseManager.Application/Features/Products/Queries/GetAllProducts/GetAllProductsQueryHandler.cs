using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;
using WarehouseManager.Domain.Iterators;

namespace WarehouseManager.Application.Features.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllProductsQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var all = await _uow.Products.GetAllAsync(ct);

        Func<Product, bool> filter = p =>
        {
            if (!p.IsActive && !request.IncludeInactive) return false;
            if (request.OnlyInStock && (p.Stock == null || p.Stock.QuantityOnHand <= 0)) return false;

            var matchSearch = string.IsNullOrWhiteSpace(request.Search)
                              || p.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase)
                              || p.Sku.Contains(request.Search, StringComparison.OrdinalIgnoreCase);
            var matchCategory = !request.CategoryId.HasValue || p.CategoryId == request.CategoryId.Value;
            
            return matchSearch && matchCategory;
        };

        Func<Product, object> orderBy = request.SortBy?.ToLower() switch
        {
            "name" => p => p.Name,
            "price" => p => p.Price,
            _ => p => p.Name
        };

        var collection = new ProductCollection(all, filter, orderBy, request.SortDescending);
        var iterator = collection.CreatePagedIterator(request.Page, request.PageSize);

        var items = new List<ProductDto>();
        while (iterator.HasNext())
        {
            var p = iterator.Next();
            items.Add(new ProductDto(
                p.Id, p.Name, p.Description, p.Sku, p.Price,
                p.CategoryId, p.Category.Name, p.SupplierId,
                p.Stock?.QuantityOnHand ?? 0));
        }

        return new PagedResult<ProductDto>(items, collection.Count, request.Page, request.PageSize);
    }
}