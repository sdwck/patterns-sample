using MediatR;
using WarehouseManager.Domain.Common;

namespace WarehouseManager.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string? Description, Guid? ParentCategoryId) : IRequest<Result<Guid>>;