using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems;

public class GetCartItemsRequestValidator : AbstractValidator<GetCartItemsRequest>
{
    public GetCartItemsRequestValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}

