using FluentValidation;

namespace WarehouseManager.Application.Features.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Action).NotEmpty()
            .Must(a => new[] { "confirm", "process", "ship", "deliver", "cancel" }
                .Contains(a.ToLower()))
            .WithMessage("Action must be one of: confirm, process, ship, deliver, cancel.");
    }
}