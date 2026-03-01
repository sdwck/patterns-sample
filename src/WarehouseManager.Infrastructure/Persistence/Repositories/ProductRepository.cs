using Microsoft.EntityFrameworkCore;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await DbSet.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Stock)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public override async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbSet.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Stock)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        return await DbSet.Include(p => p.Category).Include(p => p.Stock)
            .Where(p => p.CategoryId == categoryId).ToListAsync(ct);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        return await DbSet.FirstOrDefaultAsync(p => p.Sku == sku, ct);
    }
}