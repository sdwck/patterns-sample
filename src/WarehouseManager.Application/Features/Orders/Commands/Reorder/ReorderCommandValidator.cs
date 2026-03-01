using FluentValidation;

namespace WarehouseManager.Application.Features.Orders.Commands.Reorder;

public class ReorderCommandValidator : AbstractValidator<ReorderCommand>
{
    public ReorderCommandValidator()
    {
        RuleFor(x => x.OriginalOrderId).NotEmpty();
    }
}