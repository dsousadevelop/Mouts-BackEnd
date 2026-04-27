using Ambev.DeveloperEvaluation.Common.Security;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Common.Security;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher;

    public BCryptPasswordHasherTests()
    {
        _hasher = new BCryptPasswordHasher();
    }

    [Fact(DisplayName = "HashPassword deve retornar uma string hashada")]
    public void HashPassword_ShouldReturnHashedString()
    {
        // Arrange
        const string password = "Password123!";

        // Act
        var hash = _hasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(password);
    }

    [Fact(DisplayName = "VerifyPassword deve retornar verdadeiro para senha correta")]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        // Arrange
        const string password = "Password123!";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "VerifyPassword deve retornar falso para senha incorreta")]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        // Arrange
        const string password = "Password123!";
        var hash = _hasher.HashPassword(password);
        const string incorrectPassword = "WrongPassword!";

        // Act
        var result = _hasher.VerifyPassword(incorrectPassword, hash);

        // Assert
        result.Should().BeFalse();
    }
}
