using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    Guid? CategoryId = null,
    string? SortBy = null,
    bool SortDescending = false)
    : IRequest<PagedResult<ProductDto>>;