using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Stock.Commands.RestockProduct;

public record RestockProductCommand(Guid ProductId, int Quantity) : IRequest<Result>;