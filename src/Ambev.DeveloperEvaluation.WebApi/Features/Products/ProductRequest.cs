using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    /// <summary>
    /// Represents the request for product operations.
    /// </summary>
    public class ProductRequest
    {
        /// <summary>
        /// The unique identifier of the product. Optional for creation, required for updates.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The title of the product.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The category identifier for the product.
        /// </summary>
        public int Category_Id { get; set; }

        /// <summary>
        /// The image URL of the product.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// The rating information of the product.
        /// </summary>
        public RatingRequest Rating { get; set; } = new();
    }
}
