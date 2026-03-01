using Microsoft.Extensions.Logging;
using WarehouseManager.Application.Common;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : BaseCommandHandler<CreateProductCommand, Guid>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IUnitOfWork uow, ILogger<CreateProductCommandHandler> logger) : base(uow)
    {
        _logger = logger;
    }

    protected override async Task<Result> ValidateAsync(CreateProductCommand request, CancellationToken ct)
    {
        var existing = await Uow.Products.GetBySkuAsync(request.Sku, ct);
        if (existing is not null)
            return Result.Failure("Product with this SKU already exists.");

        var category = await Uow.Categories.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            return Result.Failure("Category not found.");

        return Result.Success();
    }

    protected override async Task<Result<Guid>> ExecuteAsync(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Sku = request.Sku,
            Price = request.Price,
            CategoryId = request.CategoryId,
            SupplierId = request.SupplierId
        };

        await Uow.Products.AddAsync(product, ct);
        await Uow.Stock.AddAsync(
            new Domain.Entities.Stock { ProductId = product.Id, QuantityOnHand = request.InitialStock }, ct);

        return Result.Success(product.Id);
    }

    protected override Task PostExecuteAsync(CreateProductCommand request, Guid result, CancellationToken ct)
    {
        _logger.LogInformation("Product created: {Id}, SKU: {Sku}", result, request.Sku);
        return Task.CompletedTask;
    }
}