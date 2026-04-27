using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the PasswordValidator class.
/// Tests cover password requirements including length, character types, and empty checks.
/// </summary>
public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator;

    public PasswordValidatorTests()
    {
        _validator = new PasswordValidator();
    }

    /// <summary>
    /// Tests that validation passes for various valid password formats.
    /// </summary>
    [Fact(DisplayName = "Senhas válidas devem passar na validação")]
    public void Given_ValidPassword_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var password = UserTestData.GenerateValidPassword();

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails when the password is empty.
    /// </summary>
    [Fact(DisplayName = "Senha vazia deve falhar na validação")]
    public void Given_EmptyPassword_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        const string password = "";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Tests that validation fails when password is shorter than minimum length.
    /// </summary>
    /// <param name="password">The short password to test.</param>
    [Theory(DisplayName = "Senha mais curta que o comprimento mínimo deve falhar na validação")]
    [InlineData("Test@1")] // 6 characters
    [InlineData("Pass#2")] // 7 characters
    public void Given_PasswordShorterThanMinimum_When_Validated_Then_ShouldHaveError(string password)
    {
        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x);
    }

    /// <summary>
    /// Tests that validation fails when password lacks uppercase letters.
    /// </summary>
    [Fact(DisplayName = "Senha sem letras maiúsculas deve falhar na validação")]
    public void Given_PasswordWithoutUppercase_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        const string password = "password@123";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }

    /// <summary>
    /// Tests that validation fails when password lacks lowercase letters.
    /// </summary>
    [Fact(DisplayName = "Senha sem letras minúsculas deve falhar na validação")]
    public void Given_PasswordWithoutLowercase_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        const string password = "PASSWORD@123";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }

    /// <summary>
    /// Tests that validation fails when password lacks numbers.
    /// </summary>
    [Fact(DisplayName = "Senha sem números deve falhar na validação")]
    public void Given_PasswordWithoutNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        const string password = "Password@ABC";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Password must contain at least one number.");
    }

    /// <summary>
    /// Tests that validation fails when password lacks special characters.
    /// </summary>
    [Fact(DisplayName = "Senha sem caracteres especiais deve falhar na validação")]
    public void Given_PasswordWithoutSpecialCharacter_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        const string password = "Password123";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Password must contain at least one special character.");
    }
}
