namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.CreateCartItem;

public class CreateCartItemRequest
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
