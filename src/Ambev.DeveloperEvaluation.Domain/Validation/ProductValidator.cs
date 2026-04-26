using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    internal class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(ad => ad.Title)
           .NotEmpty()
           .WithMessage("The description cannot be empty.");

            RuleFor(ad => ad.Title)
           .MaximumLength(60)
           .WithMessage("Description cannot be longer than 100 characters.");

            RuleFor(p => p.Price)
           .GreaterThan(0)
           .WithMessage("Price must be greater than zero.");

            RuleFor(p => p.CategoryId)
           .NotEqual(0)
           .WithMessage("Category cannot be zeroed out.");

            RuleFor(ad => ad.Description)
           .NotEmpty()
           .WithMessage("The description cannot be empty.");

            RuleFor(ad => ad.Description)
           .MaximumLength(100)
           .WithMessage("Description cannot be longer than 100 characters.");
        }
    }
}
