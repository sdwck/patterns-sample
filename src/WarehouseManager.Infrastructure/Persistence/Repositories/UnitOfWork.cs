using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(
        AppDbContext context,
        IProductRepository products,
        ICategoryRepository categories,
        IOrderRepository orders,
        IStockRepository stock)
    {
        _context = context;
        Products = products;
        Categories = categories;
        Orders = orders;
        Stock = stock;
        Customers = new GenericRepository<Customer>(context);
        Suppliers = new GenericRepository<Supplier>(context);
    }

    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }
    public IOrderRepository Orders { get; }
    public IStockRepository Stock { get; }
    public IRepository<Customer> Customers { get; }
    public IRepository<Supplier> Suppliers { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}