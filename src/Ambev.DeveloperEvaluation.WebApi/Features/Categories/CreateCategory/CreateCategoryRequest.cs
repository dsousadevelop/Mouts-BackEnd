namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory
{
    /// <summary>
    /// Represents the request to create a category.
    /// </summary>
    public class CreateCategoryRequest
    {
        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
