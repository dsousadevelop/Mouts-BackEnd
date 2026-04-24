namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories
{
    /// <summary>
    /// Represents the request for category operations.
    /// </summary>
    public class CategoryRequest
    {
        /// <summary>
        /// The unique identifier of the category. Optional for creation, required for updates.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
