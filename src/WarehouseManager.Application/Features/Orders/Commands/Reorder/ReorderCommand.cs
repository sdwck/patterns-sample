using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Orders.Commands.Reorder;

public record ReorderCommand(Guid OriginalOrderId) : IRequest<Result<Guid>>;