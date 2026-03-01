using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Orders.Queries.ResolveCustomerId;

public record ResolveCustomerIdQuery(Guid UserId) : IRequest<Result<Guid>>;