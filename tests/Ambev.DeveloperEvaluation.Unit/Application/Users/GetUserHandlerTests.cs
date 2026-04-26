using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users;

public class GetUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserHandler(_userRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid id When user exists Then returns user details")]
    public async Task Handle_ExistingUser_ReturnsGetUserResult()
    {
        // Given
        var userId = 1;
        var command = new GetUserCommand(userId);
        var user = new User("testuser", "test@example.com", "123", "password", "First", "Last", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer, Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Active);
        user.Id = userId;
        var resultDto = new GetUserResult { Id = userId, Username = "testuser" };

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(resultDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(userId);
        result.AsT0.Username.Should().Be("testuser");
    }

    [Fact(DisplayName = "Given nonexistent id When user does not exist Then returns NotFoundError")]
    public async Task Handle_NonExistentUser_ReturnsNotFoundError()
    {
        // Given
        var userId = 1;
        var command = new GetUserCommand(userId);

        _userRepository.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((User?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain($"User with ID {userId} not found");
    }

    [Fact(DisplayName = "Given invalid id When getting user Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = new GetUserCommand(0);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}
