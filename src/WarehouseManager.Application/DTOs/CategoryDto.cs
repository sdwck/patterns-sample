namespace WarehouseManager.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string? Description,
    Guid? ParentCategoryId,
    List<CategoryDto> SubCategories);