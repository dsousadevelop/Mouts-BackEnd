using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory
{
    /// <summary>
    /// Validator for CreateCategoryRequest
    /// </summary>
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CreateCategoryRequestValidator class.
        /// </summary>
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
        }
    }
}
