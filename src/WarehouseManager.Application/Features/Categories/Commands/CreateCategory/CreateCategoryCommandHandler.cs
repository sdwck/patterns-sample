using MediatR;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly IUnitOfWork _uow;

    public CreateCategoryCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        if (request.ParentCategoryId.HasValue)
        {
            var parent = await _uow.Categories.GetByIdAsync(request.ParentCategoryId.Value, ct);
            if (parent is null) return Result.Failure<Guid>("Parent category not found.");
        }

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId
        };

        await _uow.Categories.AddAsync(category, ct);
        await _uow.SaveChangesAsync(ct);
        return Result.Success(category.Id);
    }
}