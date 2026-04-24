using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    internal class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(ad => ad.Description)
           .NotEmpty()
           .WithMessage("Description not permission empty");

            RuleFor(ad => ad.Description)
           .MaximumLength(40)
           .WithMessage("Description cannot be longer than 40 characters.");
        }
    }
}
