using Microsoft.Extensions.Logging;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Infrastructure.Persistence.Decorators;

using Microsoft.Extensions.Logging;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

public class LoggingProductRepositoryDecorator : ProductRepositoryDecorator
{
    private readonly ILogger<LoggingProductRepositoryDecorator> _logger;

    public LoggingProductRepositoryDecorator(
        IProductRepository component,
        ILogger<LoggingProductRepositoryDecorator> logger)
        : base(component)
    {
        _logger = logger;
    }

    public override async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductById: {Id}", id);
        return await base.GetByIdAsync(id, ct);
    }

    public override async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        _logger.LogDebug("GetAllProducts");
        return await base.GetAllAsync(ct);
    }

    public override async Task<Product> AddAsync(Product entity, CancellationToken ct = default)
    {
        _logger.LogInformation("AddProduct: {Name} ({Sku})", entity.Name, entity.Sku);
        return await base.AddAsync(entity, ct);
    }

    public override void Update(Product entity)
    {
        _logger.LogInformation("UpdateProduct: {Id}", entity.Id);
        base.Update(entity);
    }

    public override void Delete(Product entity)
    {
        _logger.LogInformation("DeleteProduct: {Id}", entity.Id);
        base.Delete(entity);
    }

    public override async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductsByCategory: {CategoryId}", categoryId);
        return await base.GetByCategoryIdAsync(categoryId, ct);
    }

    public override async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        _logger.LogDebug("GetProductBySku: {Sku}", sku);
        return await base.GetBySkuAsync(sku, ct);
    }
}

public abstract class ProductRepositoryDecorator : IProductRepository
{
    protected readonly IProductRepository _component;

    protected ProductRepositoryDecorator(IProductRepository component)
    {
        _component = component;
    }
   
    public virtual Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return _component.GetByIdAsync(id, ct);
    }

    public virtual Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        return _component.GetAllAsync(ct);
    }

    public virtual Task<Product> AddAsync(Product entity, CancellationToken ct = default)
    {
        return _component.AddAsync(entity, ct);
    }

    public virtual void Update(Product entity)
    {
        _component.Update(entity);
    }

    public virtual void Delete(Product entity)
    {
        _component.Delete(entity);
    }

    public virtual Task<IReadOnlyList<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        return _component.GetByCategoryIdAsync(categoryId, ct);
    }

    public virtual Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        return _component.GetBySkuAsync(sku, ct);
    }
}