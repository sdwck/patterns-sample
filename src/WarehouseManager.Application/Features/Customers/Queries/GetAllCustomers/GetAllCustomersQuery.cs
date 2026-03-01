using MediatR;
using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Features.Customers.Queries.GetAllCustomers;

public record GetAllCustomersQuery : IRequest<List<CustomerDto>>;