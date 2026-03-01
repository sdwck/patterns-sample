using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllCustomersQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken ct)
    {
        var all = await _uow.Customers.GetAllAsync(ct);
        return all.Select(c => new CustomerDto(c.Id, c.FirstName, c.LastName, c.Email, c.Phone, c.Address)).ToList();
    }
}