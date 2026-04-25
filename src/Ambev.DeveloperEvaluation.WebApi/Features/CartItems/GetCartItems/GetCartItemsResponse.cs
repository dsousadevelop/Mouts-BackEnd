namespace Ambev.DeveloperEvaluation.WebApi.Features.CartItems.GetCartItems;

public class GetCartItemsResponse
{
    public List<GetCartItemsItemResponse> Items { get; set; } = new();
}

public class GetCartItemsItemResponse
{
    public int? Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
}

