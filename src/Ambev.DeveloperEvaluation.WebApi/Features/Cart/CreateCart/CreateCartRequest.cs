using Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.CreateCart;

public class CreateCartRequest
{
    public int UserId { get; set; }
    public List<CreateCartItemRequest> CartItems { get; set; } = new();
}

