using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
    }
}
