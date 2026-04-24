using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Products.DTOs
{
    public class ProductDto
    {
        public ProductDto() { }

        public ProductDto(int? id, string title, decimal price, string description, int categoryId, string? image, RatingDto rating)
        {
            Id = id;
            Title = title;
            Price = price;
            Description = description;
            CategoryId = categoryId;
            Image = image;
            Rating = rating;
        }

        public int? Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string? Image { get; set; }
        public RatingDto Rating { get; set; }
    }
}
