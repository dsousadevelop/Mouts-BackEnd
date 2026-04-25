using Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.ListCarts;

public class ListCartsResponse
{
    public List<GetCartResponse> Carts { get; set; } = new();
}

