using Ambev.DeveloperEvaluation.Common.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Common.Security;

public class JwtTokenGeneratorTests
{
    private readonly IConfiguration _configuration;
    private readonly JwtTokenGenerator _jwtGenerator;

    public JwtTokenGeneratorTests()
    {
        _configuration = Substitute.For<IConfiguration>();
        _configuration["Jwt:SecretKey"].Returns("super_secret_key_123_abc_456_def_789");
        _jwtGenerator = new JwtTokenGenerator(_configuration);
    }

    [Fact(DisplayName = "GenerateToken should return a valid JWT token")]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var user = Substitute.For<IUser>();
        user.Id.Returns("1");
        user.Username.Returns("testuser");
        user.Role.Returns("Admin");

        // Act
        var token = _jwtGenerator.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.Should().Contain(c => c.Type == "nameid" && c.Value == "1");
        jwtToken.Claims.Should().Contain(c => c.Type == "unique_name" && c.Value == "testuser");
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == "Admin");
    }

    [Fact(DisplayName = "ValidateToken should return ClaimsPrincipal for valid token")]
    public void ValidateToken_ShouldReturnPrincipal_ForValidToken()
    {
        // Arrange
        var user = Substitute.For<IUser>();
        user.Id.Returns("1");
        user.Username.Returns("testuser");
        user.Role.Returns("Admin");

        var token = _jwtGenerator.GenerateToken(user);

        // Act
        var principal = _jwtGenerator.ValidateToken(token);

        // Assert
        principal.Should().NotBeNull();
        principal!.Identity!.IsAuthenticated.Should().BeTrue();
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "1");
    }

    [Fact(DisplayName = "ValidateToken should return null for invalid token")]
    public void ValidateToken_ShouldReturnNull_ForInvalidToken()
    {
        // Arrange
        const string invalidToken = "invalid.token.here";

        // Act
        var principal = _jwtGenerator.ValidateToken(invalidToken);

        // Assert
        principal.Should().BeNull();
    }

    [Fact(DisplayName = "GenerateToken should throw exception when secret key is not configured")]
    public void GenerateToken_ShouldThrowException_WhenSecretKeyIsMissing()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:SecretKey"].Returns((string?)null);
        var jwtGenerator = new JwtTokenGenerator(configuration);
        var user = Substitute.For<IUser>();

        // Act
        var act = () => jwtGenerator.GenerateToken(user);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("JWT Secret Key is not configured.");
    }

    [Fact(DisplayName = "ValidateToken should throw exception when secret key is not configured")]
    public void ValidateToken_ShouldThrowException_WhenSecretKeyIsMissing()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:SecretKey"].Returns((string?)null);
        var jwtGenerator = new JwtTokenGenerator(configuration);

        // Act
        var act = () => jwtGenerator.ValidateToken("any-token");

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("JWT Secret Key is not configured.");
    }
}
