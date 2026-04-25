namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory
{
    /// <summary>
    /// API response model for create category operation
    /// </summary>
    public class CreateCategoryResponse
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
