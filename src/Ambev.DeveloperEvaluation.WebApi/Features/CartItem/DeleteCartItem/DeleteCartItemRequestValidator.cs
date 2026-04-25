using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.DeleteCartItem;

public class DeleteCartItemRequestValidator : AbstractValidator<DeleteCartItemRequest>
{
    public DeleteCartItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
