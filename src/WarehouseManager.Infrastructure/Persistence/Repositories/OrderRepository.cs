using Microsoft.EntityFrameworkCore;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken ct = default)
    {
        return await DbSet.Include(o => o.Customer).Include(o => o.Items).ThenInclude(i => i.Product)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
    {
        return await DbSet.Include(o => o.Items).ThenInclude(i => i.Product)
            .Where(o => o.CustomerId == customerId).ToListAsync(ct);
    }

    public async Task<Order?> GetWithItemsAsync(Guid id, CancellationToken ct = default)
    {
        return await DbSet.Include(o => o.Customer).Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id, ct);
    }
}