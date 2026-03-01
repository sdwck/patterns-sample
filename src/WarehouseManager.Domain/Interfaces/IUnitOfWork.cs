using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IOrderRepository Orders { get; }
    IStockRepository Stock { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Supplier> Suppliers { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}