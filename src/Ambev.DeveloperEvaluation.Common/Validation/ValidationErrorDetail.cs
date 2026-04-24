using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Validation;

public class ValidationErrorDetail
{
    public string Code { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    public static explicit operator ValidationErrorDetail(ValidationFailure validationFailure)
    {
        return new ValidationErrorDetail
        {
            Description = validationFailure.ErrorMessage,
            Code = validationFailure.ErrorCode
        };
    }
}
