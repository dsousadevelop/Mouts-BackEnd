using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.DeleteCartItem;

public class DeleteCartItemRequestValidator : AbstractValidator<DeleteCartItemRequest>
{
    public DeleteCartItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
