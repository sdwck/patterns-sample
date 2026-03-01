using WarehouseManager.Domain.Entities;

namespace WarehouseManager.Domain.Interfaces;

public interface IStockRepository : IRepository<Stock>
{
    Task<Stock?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<IReadOnlyList<Stock>> GetLowStockAsync(CancellationToken ct = default);
}