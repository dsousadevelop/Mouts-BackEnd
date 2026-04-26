using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    internal class CartValidator : AbstractValidator<Cart>
    {
        public CartValidator()
        {
            RuleForEach(x => x.CartItems).SetValidator(new CartItemValidator());

            RuleFor(ad => ad.UserId)
           .NotEqual(0)
           .WithMessage("Number invalid not permission zero.");
        }
    }
}
