using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public UpdateProductCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _uow.Products.GetByIdAsync(request.Id, ct);
        if (product is null)
            return Result.Failure("Product not found.");

        var category = await _uow.Categories.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            return Result.Failure("Category not found.");

        if (request.SupplierId.HasValue)
        {
            var supplier = await _uow.Suppliers.GetByIdAsync(request.SupplierId.Value, ct);
            if (supplier is null)
                return Result.Failure("Supplier not found.");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;
        product.SupplierId = request.SupplierId;
        product.UpdatedAt = DateTime.UtcNow;

        _uow.Products.Update(product);
        await _uow.SaveChangesAsync(ct);

        return Result.Success();
    }
}