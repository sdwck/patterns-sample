using MediatR;
using WarehouseManager.Application.DTOs;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllCategoriesQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        var roots = await _uow.Categories.GetRootCategoriesAsync(ct);
        return roots.Select(Map).ToList();
    }

    private static CategoryDto Map(Category c)
    {
        return new CategoryDto(
            c.Id, 
            c.Name, 
            c.Description, 
            c.ParentCategoryId,
            c.Children.OfType<Category>().Select(Map).ToList()
        );
    }
}