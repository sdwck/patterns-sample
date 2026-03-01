using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;