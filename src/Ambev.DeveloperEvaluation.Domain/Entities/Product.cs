using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product: BaseEntity
    {
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public string? Image { get; private set; }
        public decimal? Rating_Rate { get; private set; }
        public short? Rating_Count { get; private set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<CartItem>? CartItems { get; set; }
        public Product() { }
        /// <summary>
        /// Entity of Product
        /// </summary>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="image"></param>
        /// <param name="rating_Rate"></param>
        /// <param name="rating_Count"></param>
        /// <param name="category"></param>
        public Product(string title, decimal price, string description, int categoryId, string image, decimal rating_Rate, short rating_Count, Category category, DateTime createdAt, DateTime? updatedAt)
        {
            Title = title;
            Price = price;
            Description = description;
            CategoryId = categoryId;
            Image = image;
            Rating_Rate = rating_Rate;
            Rating_Count = rating_Count;
            Category = category;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

    }
}
