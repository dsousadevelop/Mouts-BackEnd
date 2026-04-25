using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs
{
    public class CartItemDto
    {
        public int? Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? Discount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string? ProductDescription { get; set; }
    }
}
