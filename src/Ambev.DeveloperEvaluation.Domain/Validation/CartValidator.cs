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
        public CartValidator() {

            RuleFor(ad => ad.UserId)
           .NotEqual(0)
           .WithMessage("Number invalid not permission zero.");

           // RuleFor(ad => ad.Date)
           //.NotNull()
           //.WithMessage("Date invalid value null");

           // RuleFor(x => x.Date)
           //.Must(d => d != default(DateTime))
           //.WithMessage("A data não pode ser vazia.");
        }
    }
}
