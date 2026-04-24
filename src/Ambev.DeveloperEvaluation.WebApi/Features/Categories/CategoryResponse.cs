namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories
{
    /// <summary>
    /// API response model for category operations
    /// </summary>
    public class CategoryResponse
    {
        /// <summary>
        /// The unique identifier of the category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
