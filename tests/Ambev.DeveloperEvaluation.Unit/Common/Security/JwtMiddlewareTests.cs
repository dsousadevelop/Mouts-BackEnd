using Ambev.DeveloperEvaluation.Common.Security;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Common.Security;

public class JwtMiddlewareTests
{
    private readonly RequestDelegate _next;
    private readonly IJwtTokenGenerator _jwtService;
    private readonly JwtMiddleware _middleware;

    public JwtMiddlewareTests()
    {
        _next = Substitute.For<RequestDelegate>();
        _jwtService = Substitute.For<IJwtTokenGenerator>();
        _middleware = new JwtMiddleware(_next);
    }

    [Fact(DisplayName = "Invoke should set User in HttpContext when token is valid")]
    public async Task Invoke_ShouldSetUser_WhenTokenIsValid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        const string token = "valid-token";
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser")
        }, "jwt"));

        _jwtService.ValidateToken(token).Returns(principal);

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        context.User.Should().Be(principal);
        await _next.Received(1)(context);
    }

    [Fact(DisplayName = "Invoke should not set User in HttpContext when token is missing")]
    public async Task Invoke_ShouldNotSetUser_WhenTokenIsMissing()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        context.User.Identity!.IsAuthenticated.Should().BeFalse();
        await _next.Received(1)(context);
    }

    [Fact(DisplayName = "Invoke should call next even if token is invalid")]
    public async Task Invoke_ShouldCallNext_WhenTokenIsInvalid()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer invalid-token";
        _jwtService.ValidateToken(Arg.Any<string>()).Returns((ClaimsPrincipal?)null);

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        await _next.Received(1)(context);
    }

    [Fact(DisplayName = "Invoke should generate new token and add header when token is expired but user is found")]
    public async Task Invoke_ShouldGenerateNewToken_WhenTokenIsExpiredButUserFound()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("a-very-long-and-secure-secret-key-that-is-at-least-256-bits-long-for-testing-purposes");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddMinutes(-30), // Expired
            NotBefore = DateTime.UtcNow.AddMinutes(-60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handler.CreateToken(tokenDescriptor);
        var tokenString = handler.WriteToken(token);

        context.Request.Headers["Authorization"] = $"Bearer {tokenString}";
        _jwtService.ValidateToken(tokenString).Returns((ClaimsPrincipal?)null);
        _jwtService.GenerateToken(Arg.Any<Ambev.DeveloperEvaluation.Common.Security.IUser>()).Returns("new-token");

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        context.Response.Headers.ContainsKey("X-New-Token").Should().BeTrue();
        context.Response.Headers["X-New-Token"].ToString().Should().Be("new-token");
        await _next.Received(1)(context);
    }

    [Fact(DisplayName = "Invoke should not generate new token when token is not expired but ValidateToken returns null")]
    public async Task Invoke_ShouldNotGenerateNewToken_WhenTokenIsNotExpired()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("a-very-long-and-secure-secret-key-that-is-at-least-256-bits-long-for-testing-purposes");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddMinutes(30), // Not expired
            NotBefore = DateTime.UtcNow.AddMinutes(-30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = handler.CreateToken(tokenDescriptor);
        var tokenString = handler.WriteToken(token);

        context.Request.Headers["Authorization"] = $"Bearer {tokenString}";
        _jwtService.ValidateToken(tokenString).Returns((ClaimsPrincipal?)null);

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        context.Response.Headers.ContainsKey("X-New-Token").Should().BeFalse();
        await _next.Received(1)(context);
    }

    [Fact(DisplayName = "GetUserIdFromExpiredToken should return null when token is malformed")]
    public async Task Invoke_ShouldNotSetUser_WhenTokenIsMalformed()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer malformed-token";
        _jwtService.ValidateToken("malformed-token").Returns((ClaimsPrincipal?)null);

        // Act
        await _middleware.Invoke(context, _jwtService);

        // Assert
        context.Response.Headers.ContainsKey("X-New-Token").Should().BeFalse();
        await _next.Received(1)(context);
    }
}
