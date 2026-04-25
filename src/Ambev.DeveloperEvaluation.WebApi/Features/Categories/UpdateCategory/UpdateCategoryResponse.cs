namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory
{
    /// <summary>
    /// API response model for update category operation
    /// </summary>
    public class UpdateCategoryResponse
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
