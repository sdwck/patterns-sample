using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default);
    Task<Category?> GetWithSubCategoriesAsync(Guid id, CancellationToken ct = default);
}