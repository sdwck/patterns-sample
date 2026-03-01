using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Suppliers.Commands.CreateSupplier;

public record CreateSupplierCommand(string Name, string? ContactEmail, string? Phone, string? Address)
    : IRequest<Result<Guid>>;