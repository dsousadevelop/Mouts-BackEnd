using Ambev.DeveloperEvaluation.Application.Features.CartItems.DTOs;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Features.Carts.DTOs
{
    public class CartDto
    {
        public CartDto(int? id, int userId, DateTime? date, IList<CartItemDto>? cartItems)
        {
            Id = id;
            UserId = userId;
            Date = date;
            CartItems = cartItems;
        }

        public int? Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public virtual IList<CartItemDto>? CartItems { get; set; }

    }
}
