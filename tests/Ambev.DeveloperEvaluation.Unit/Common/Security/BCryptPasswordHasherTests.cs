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

    [Fact(DisplayName = "HashPassword should return a hashed string")]
    public void HashPassword_ShouldReturnHashedString()
    {
        // Arrange
        var password = "Password123!";

        // Act
        var hash = _hasher.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(password);
    }

    [Fact(DisplayName = "VerifyPassword should return true for correct password")]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        // Arrange
        var password = "Password123!";
        var hash = _hasher.HashPassword(password);

        // Act
        var result = _hasher.VerifyPassword(password, hash);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "VerifyPassword should return false for incorrect password")]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        // Arrange
        var password = "Password123!";
        var hash = _hasher.HashPassword(password);
        var incorrectPassword = "WrongPassword!";

        // Act
        var result = _hasher.VerifyPassword(incorrectPassword, hash);

        // Assert
        result.Should().BeFalse();
    }
}
