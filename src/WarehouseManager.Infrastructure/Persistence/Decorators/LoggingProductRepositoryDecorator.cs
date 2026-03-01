using Microsoft.Extensions.Logging;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Decorators;

public class LoggingProductRepositoryDecorator : IProductRepository
{
    private readonly IProductRepository _inner;
    private readonly ILogger<LoggingProductRepositoryDecorator> _logger;

    public LoggingProductRepositoryDecorator(IProductRepository inner,
        ILogger<LoggingProductRepositoryDecorator> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductById: {Id}", id);
        return await _inner.GetByIdAsync(id, ct);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetAllProducts");
        return await _inner.GetAllAsync(ct);
    }

    public async Task<Product> AddAsync(Product entity, CancellationToken ct = default)
    {
        _logger.LogInformation("AddProduct: {Name} ({Sku})", entity.Name, entity.Sku);
        return await _inner.AddAsync(entity, ct);
    }

    public void Update(Product entity)
    {
        _logger.LogInformation("UpdateProduct: {Id}", entity.Id);
        _inner.Update(entity);
    }

    public void Delete(Product entity)
    {
        _logger.LogInformation("DeleteProduct: {Id}", entity.Id);
        _inner.Delete(entity);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductsByCategory: {CategoryId}", categoryId);
        return await _inner.GetByCategoryIdAsync(categoryId, ct);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductBySku: {Sku}", sku);
        return await _inner.GetBySkuAsync(sku, ct);
    }
}