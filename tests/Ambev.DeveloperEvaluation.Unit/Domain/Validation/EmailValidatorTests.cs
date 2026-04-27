using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the EmailValidator class.
/// Tests cover various email validation scenarios including format, length, and empty checks.
/// </summary>
public class EmailValidatorTests
{
    private readonly EmailValidator _validator;

    public EmailValidatorTests()
    {
        _validator = new EmailValidator();
    }

    /// <summary>
    /// Tests that validation passes for various valid email formats.
    /// </summary>
    [Fact(DisplayName = "Formatos de e-mail válidos devem passar na validação")]
    public void Given_ValidEmailFormat_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var email = UserTestData.GenerateValidEmail();

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when the email is empty.
    /// </summary>
    [Fact(DisplayName = "E-mail vazio deve falhar na validação")]
    public void Given_EmptyEmail_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var email = string.Empty;

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The email address cannot be empty.");
    }

    /// <summary>
    /// Tests that validation fails for various invalid email formats.
    /// </summary>
    [Theory(DisplayName = "Formatos de e-mail inválidos devem falhar na validação")]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@.com")]
    [InlineData("user@domain.")]
    public void Given_InvalidEmailFormat_When_Validated_Then_ShouldHaveError(string email)
    {
        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The provided email address is not valid.");
    }

    /// <summary>
    /// Tests that validation fails when email exceeds maximum length.
    /// </summary>
    [Fact(DisplayName = "E-mail excedendo o comprimento máximo deve falhar na validação")]
    public void Given_EmailExceeding100Characters_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var email = $"{"a".PadLeft(90, 'a')}@example.com"; // Creates email > 100 chars

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("The email address cannot be longer than 100 characters.");
    }
}
