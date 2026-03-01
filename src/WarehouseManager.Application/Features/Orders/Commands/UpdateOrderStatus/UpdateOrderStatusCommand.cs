using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(Guid OrderId, string Action) : IRequest<Result>;