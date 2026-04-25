namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.UpdateCartItem;

public class UpdateCartItemRequest
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

