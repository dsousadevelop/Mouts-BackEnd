using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users;

public class DeleteUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new DeleteUserHandler(_userRepository);
    }

    [Fact(DisplayName = "Dado um ID válido, ao deletar o usuário, retorna sucesso")]
    public async Task Handle_ValidRequest_ReturnsDeleteUserResponse()
    {
        // Given
        const int userId = 1;
        var command = new DeleteUserCommand(userId);

        _userRepository.DeleteAsync(userId, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Success.Should().BeTrue();
        await _userRepository.Received(1).DeleteAsync(userId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Dado um ID inexistente, ao deletar o usuário, retorna NotFoundError")]
    public async Task Handle_NonExistentUser_ReturnsNotFoundError()
    {
        // Given
        const int userId = 1;
        var command = new DeleteUserCommand(userId);

        _userRepository.DeleteAsync(userId, Arg.Any<CancellationToken>()).Returns(false);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain($"User with ID {userId} not found");
    }

    [Fact(DisplayName = "Dado um ID inválido, ao deletar o usuário, lança uma exceção de validação")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = new DeleteUserCommand(0);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }
}
