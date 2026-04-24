namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Common
{
    /// <summary>
    /// Represents the rating information in the product request.
    /// </summary>
    public class RatingRequest
    {
        /// <summary>
        /// The rate of the product.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The number of ratings.
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// Represents the rating information in the product response.
    /// </summary>
    public class RatingResponse
    {
        /// <summary>
        /// The rate of the product.
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// The number of ratings.
        /// </summary>
        public int Count { get; set; }
    }
}
