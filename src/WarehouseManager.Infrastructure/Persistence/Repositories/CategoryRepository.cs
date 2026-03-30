using Microsoft.EntityFrameworkCore;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
    {
        return await DbSet.Where(c => c.ParentCategoryId == null)
            .Include(c => c.Children)
            .ToListAsync(ct);
    }

    public async Task<Category?> GetWithSubCategoriesAsync(Guid id, CancellationToken ct = default)
    {
        return await DbSet.Include(c => c.Children)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
}