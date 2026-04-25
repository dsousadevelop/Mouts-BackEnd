using FluentValidation;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.Common.Validation;

public static class Validator
{
    public static async Task<ValidationResultDetail> ValidateAsync<T>(T instance)
    {
        var validatorInterface = typeof(IValidator<>).MakeGenericType(typeof(T));
        var validatorType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t.IsClass && !t.IsAbstract && validatorInterface.IsAssignableFrom(t));

        if (validatorType == null)
        {
            // Fallback: search in the assembly of the entity itself
            validatorType = typeof(T).Assembly
                .GetTypes()
                .FirstOrDefault(t => t.IsClass && !t.IsAbstract && validatorInterface.IsAssignableFrom(t));
        }

        if (validatorType == null)
        {
            throw new InvalidOperationException($"No validator found for: {typeof(T).Name}");
        }

        var validator = (IValidator)Activator.CreateInstance(validatorType)!;
        var result = await validator.ValidateAsync(new ValidationContext<T>(instance));

        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
