using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    Guid? SupplierId)
    : IRequest<Result>;