using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth;

public class AuthenticateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthenticateUserHandler _handler;

    public AuthenticateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _handler = new AuthenticateUserHandler(_userRepository, _passwordHasher, _jwtTokenGenerator);
    }

    [Fact(DisplayName = "Given valid credentials When authenticating Then returns token")]
    public async Task Handle_ValidCredentials_ReturnsAuthenticateUserResult()
    {
        // Given
        var request = new AuthenticateUserCommand { Email = "test@example.com", Password = "Password123!" };
        var user = new User(
            "testuser",
            request.Email,
            "123",
            "hashed_password",
            "First",
            "Last",
            Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer,
            Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Active
        );
        const string token = "generated_token";

        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(request.Password, user.Password).Returns(true);
        _jwtTokenGenerator.GenerateToken(user).Returns(token);

        // When
        var result = await _handler.Handle(request, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Token.Should().Be(token);
        result.Email.Should().Be(user.Email);
    }

    [Fact(DisplayName = "Given invalid password When authenticating Then throws unauthorized access exception")]
    public async Task Handle_InvalidPassword_ThrowsUnauthorizedAccessException()
    {
        // Given
        var command = new AuthenticateUserCommand { Email = "test@example.com", Password = "wrongpassword" };
        var user = new User("testuser", "test@example.com", "123", "password", "First", "Last", UserRole.Customer, UserStatus.Active);
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(command.Password, user.Password).Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid credentials");
    }

    [Fact(DisplayName = "Given non-existent user When authenticating Then throws unauthorized access exception")]
    public async Task Handle_UserNotFound_ThrowsUnauthorizedAccessException()
    {
        // Given
        var command = new AuthenticateUserCommand { Email = "nonexistent@example.com", Password = "anypassword" };
        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("Invalid credentials");
    }

    [Fact(DisplayName = "Given inactive user When authenticating Then throws unauthorized exception")]
    public async Task Handle_InactiveUser_ThrowsUnauthorizedAccessException()
    {
        // Given
        var request = new AuthenticateUserCommand { Email = "test@example.com", Password = "Password123!" };
        var user = new User(
            "testuser",
            request.Email,
            "123",
            "hashed_password",
            "First",
            "Last",
            Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer,
            Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Inactive
        );

        _userRepository.GetByEmailAsync(request.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.VerifyPassword(request.Password, user.Password).Returns(true);

        // When
        var act = () => _handler.Handle(request, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("User is not active");
    }
}
