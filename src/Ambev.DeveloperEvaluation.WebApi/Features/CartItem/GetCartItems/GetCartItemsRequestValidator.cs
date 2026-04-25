using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.GetCartItems;

public class GetCartItemsRequestValidator : AbstractValidator<GetCartItemsRequest>
{
    public GetCartItemsRequestValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
