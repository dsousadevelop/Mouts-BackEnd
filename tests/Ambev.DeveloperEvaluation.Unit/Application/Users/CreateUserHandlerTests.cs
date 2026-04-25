using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users;

public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    [Fact(DisplayName = "Given valid user data When creating user Then returns success")]
    public async Task Handle_ValidRequest_ReturnsCreateUserResult()
    {
        // Given
        var command = new CreateUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            Phone = "+5511999999999",
            Status = Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Active,
            Role = Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer
        };
        var user = new User(command.Username, command.Email, command.Phone, command.Password, "First", "Last", command.Role, command.Status);
        user.Id = 1;
        var resultDto = new CreateUserResult { Id = user.Id.Value };

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);
        _mapper.Map<User>(command).Returns(user);
        _passwordHasher.HashPassword(command.Password).Returns("hashed_password");
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(resultDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id.Value);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given existing email When creating user Then throws invalid operation exception")]
    public async Task Handle_ExistingEmail_ThrowsInvalidOperationException()
    {
        // Given
        var command = new CreateUserCommand
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Password123!",
            Phone = "+5511999999999",
            Status = Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Active,
            Role = Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer
        };
        var existingUser = new User(command.Username, command.Email, command.Phone, command.Password, "First", "Last", Ambev.DeveloperEvaluation.Domain.Enums.UserRole.Customer, Ambev.DeveloperEvaluation.Domain.Enums.UserStatus.Active);
        existingUser.Id = 1;

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(existingUser);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"User with email {command.Email} already exists");
    }

    [Fact(DisplayName = "Given invalid command When creating user Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = new CreateUserCommand { Email = "invalid-email" };

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}
