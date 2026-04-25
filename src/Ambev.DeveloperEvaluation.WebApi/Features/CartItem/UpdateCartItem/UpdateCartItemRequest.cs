namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItem.UpdateCartItem;

public class UpdateCartItemRequest
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
