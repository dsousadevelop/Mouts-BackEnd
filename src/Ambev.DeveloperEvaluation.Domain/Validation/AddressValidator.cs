using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    internal class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(ad => ad.City)
           .NotEmpty()
           .MaximumLength(30).WithMessage("Username cannot be longer than 30 characters.");

            RuleFor(ad => ad.Street)
           .NotEmpty()
           .MaximumLength(30).WithMessage("Username cannot be longer than 30 characters.");

            RuleFor(ad => ad.Number)
           .NotEqual(0)
           .WithMessage("Number invalid not permission zero.");

            RuleFor(ad => ad.ZipCode)
           .Matches(@"^\d{5}-?\d{3}$")
           .WithMessage("ZipCode invalid.Format correct 00000-000 or 00000000.");

        }
    }
}
