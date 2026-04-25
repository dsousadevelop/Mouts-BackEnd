using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.GetCartItems;

public class GetCartItemsResponse
{
    public List<GetCartItemResponse> Items { get; set; } = new();
}
