using MediatR;
using WarehouseManager.Application.DTOs;

namespace WarehouseManager.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<List<CategoryDto>>;