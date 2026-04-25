namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.GetCategory
{
    /// <summary>
    /// API response model for get category operation
    /// </summary>
    public class GetCategoryResponse
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
