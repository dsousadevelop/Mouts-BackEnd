using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories
{
    /// <summary>
    /// Validator for CategoryRequest
    /// </summary>
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CategoryRequestValidator class.
        /// </summary>
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
        }
    }
}
