using Microsoft.EntityFrameworkCore;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class StockRepository : GenericRepository<Stock>, IStockRepository
{
    public StockRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<Stock>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbSet.Include(s => s.Product).ToListAsync(ct);
    }

    public async Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await DbSet.Include(s => s.Product).FirstOrDefaultAsync(s => s.ProductId == productId, ct);
    }

    public async Task<IReadOnlyList<Stock>> GetLowStockAsync(CancellationToken ct = default)
    {
        return await DbSet.Include(s => s.Product).Where(s => s.QuantityOnHand <= s.ReorderLevel)
            .ToListAsync(ct);
    }
}