namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory
{
    /// <summary>
    /// Represents the request to update a category.
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
