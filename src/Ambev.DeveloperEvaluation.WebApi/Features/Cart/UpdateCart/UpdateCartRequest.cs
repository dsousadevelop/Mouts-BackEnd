using Ambev.DeveloperEvaluation.WebApi.Features.CartItem.CreateCartItem;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.UpdateCart;

public class UpdateCartRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsCancelled { get; set; }
    public List<CreateCartItemRequest> CartItems { get; set; } = new();
}
