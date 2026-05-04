using MediatR;
using WarehouseManager.Application.DTOs;
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
        var allCategories = await _uow.Categories.GetAllAsync(ct);

        var lookup = allCategories.ToDictionary(
            c => c.Id, 
            c => new CategoryDto(
                c.Id, 
                c.Name, 
                c.Description, 
                c.ParentCategoryId, 
                new List<CategoryDto>()
            )
        );

        var roots = new List<CategoryDto>();

        foreach (var category in allCategories)
        {
            var dto = lookup[category.Id];

            if (category.ParentCategoryId.HasValue && lookup.TryGetValue(category.ParentCategoryId.Value, out var parentDto))
            {
                parentDto.SubCategories.Add(dto);
            }
            else
            {
                roots.Add(dto);
            }
        }

        return roots;
    }
}