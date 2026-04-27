using FluentValidation;

namespace WarehouseManager.Application.Features.Stock.Commands.RestockProduct;

public class RestockProductCommandValidator : AbstractValidator<RestockProductCommand>
{
    public RestockProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}