using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    /// <summary>
    /// Validator for ProductRequest
    /// </summary>
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        /// <summary>
        /// Initializes a new instance of the ProductRequestValidator class.
        /// </summary>
        public ProductRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Category_Id).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
        }
    }
}
