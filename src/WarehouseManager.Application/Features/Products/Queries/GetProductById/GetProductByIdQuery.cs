using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;