using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    Guid CategoryId,
    Guid? SupplierId,
    int InitialStock)
    : IRequest<Result<Guid>>;