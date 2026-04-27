using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public DeleteProductCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await _uow.Products.GetByIdAsync(request.Id, ct);
        if (product is null) return Result.Failure("Product not found.");
        
        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        
        _uow.Products.Update(product);
        await _uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}