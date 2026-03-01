using MediatR;
using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQuery : IRequest<List<SupplierDto>>;