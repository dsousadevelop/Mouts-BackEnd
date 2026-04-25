using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string Image { get; set; } = string.Empty;
    public RatingRequest Rating { get; set; } = new();
}
