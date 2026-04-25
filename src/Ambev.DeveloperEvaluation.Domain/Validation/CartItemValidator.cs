using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    internal class CartItemValidator : AbstractValidator<CartItem>
    {
        public CartItemValidator() 
        {
            RuleFor(p => p.CartId)
           .NotEqual(0)
           .WithMessage("Cart ID cannot be zeroed out.");

            RuleFor(p => p.ProductId)
           .NotEqual(0)
           .WithMessage("Product ID cannot be zeroed out.");

            RuleFor(p => p.Quantity)
           .NotEqual(0)
           .WithMessage("Quantity cannot be zeroed out.")
           .LessThanOrEqualTo(20)
           .WithMessage("Quantity cannot exceed 20 items per product.");
        }
    }
}
