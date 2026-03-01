using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? Address,
    string? Password) : IRequest<Result<Guid>>;