using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, List<SupplierDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllSuppliersQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken ct)
    {
        var all = await _uow.Suppliers.GetAllAsync(ct);
        return all.Select(s => new SupplierDto(s.Id, s.Name, s.ContactEmail, s.Phone, s.Address)).ToList();
    }
}