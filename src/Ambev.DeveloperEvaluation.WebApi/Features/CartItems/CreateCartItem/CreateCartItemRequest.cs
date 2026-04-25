namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.CreateCartItem;

public class CreateCartItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal? Discount { get; set; }
}

