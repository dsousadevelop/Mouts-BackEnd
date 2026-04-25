using System;

namespace Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs
{
    public class CartItemDto
    {
        public CartItemDto() { }

        public CartItemDto(int? id, int cartId, int productId, int quantity, decimal unitPrice, decimal discount, decimal subTotal, decimal total)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            SubTotal = subTotal;
            Total = total;
        }

        public int? Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}
