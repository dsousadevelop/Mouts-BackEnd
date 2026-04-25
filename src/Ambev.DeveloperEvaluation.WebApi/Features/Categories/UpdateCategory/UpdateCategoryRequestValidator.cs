using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Categories.UpdateCategory
{
    /// <summary>
    /// Validator for UpdateCategoryRequest
    /// </summary>
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateCategoryRequestValidator class.
        /// </summary>
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100);
        }
    }
}
