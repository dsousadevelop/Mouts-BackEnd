using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.DTOs
{
    internal class ProductDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string? Image { get; set; }
        public decimal? Rating_Rate { get; set; }
        public short? Rating_Count { get; set; }

        //public ProductDto(string title, decimal price, string description, int categoryId, string? image, decimal? rating_Rate, short? rating_Count)
        //{
        //    Title = title;
        //    Price = price;
        //    Description = description;
        //    CategoryId = categoryId;
        //    Image = image;
        //    Rating_Rate = rating_Rate;
        //    Rating_Count = rating_Count;
        //}
    }
}
