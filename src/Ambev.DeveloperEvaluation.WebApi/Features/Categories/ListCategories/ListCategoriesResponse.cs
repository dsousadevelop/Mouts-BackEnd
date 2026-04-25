namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories
{
    /// <summary>
    /// API response model for list categories operation
    /// </summary>
    public class ListCategoriesResponse
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
