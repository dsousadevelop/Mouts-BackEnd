namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.GetCart;

public class GetCartResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public DateTime Date { get; set; }
    public List<GetCartItemResponse> CartItems { get; set; } = new();
}

public class GetCartItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductDescription { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }
}

