using FluentValidation;
using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.CreateCartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleForEach(x => x.CartItems).SetValidator(new CreateCartItemRequestValidator());
    }
}
