using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Enums;
using OneOf;
using Ambev.DeveloperEvaluation.Application.Common.Errors;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Users;

public class UsersControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new UsersController(_mediator, _mapper);
    }

    [Fact(DisplayName = "CreateUser deve retornar 201 Created")]
    public async Task CreateUser_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "Password123!",
            Phone = "+5511999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };
        var resultDetail = new CreateUserResult { Id = 1 };
        var command = new CreateUserCommand { Username = "testuser" };

        _mapper.Map<CreateUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(resultDetail));
        _mapper.Map<CreateUserResponse>(resultDetail).Returns(new CreateUserResponse { Id = 1 });

        // Act
        var result = await _controller.CreateUser(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedResult>().Which.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "GetUser deve retornar 200 Ok")]
    public async Task GetUser_IdValido_DeveRetornarOk()
    {
        // Arrange
        const int userId = 1;
        var resultDetail = new GetUserResult { Id = userId, Username = "testuser" };

        _mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<OneOf<GetUserResult, NotFoundError>>(resultDetail));
        _mapper.Map<GetUserResponse>(resultDetail).Returns(new GetUserResponse { Id = userId, Username = "testuser" });

        // Act
        var result = await _controller.GetUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "GetUser deve retornar 404 NotFound quando usuário não existir")]
    public async Task GetUser_UsuarioInexistente_DeveRetornarNotFound()
    {
        // Arrange
        const int userId = 99;
        _mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<OneOf<GetUserResult, NotFoundError>>(new NotFoundError("User not found")));

        // Act
        var result = await _controller.GetUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "DeleteUser deve retornar 200 Ok")]
    public async Task DeleteUser_IdValido_DeveRetornarOk()
    {
        // Arrange
        const int userId = 1;
        _mediator.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<OneOf<DeleteUserResponse, NotFoundError>>(new DeleteUserResponse { Success = true }));

        // Act
        var result = await _controller.DeleteUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "DeleteUser deve retornar 404 NotFound quando usuário não existir")]
    public async Task DeleteUser_UsuarioInexistente_DeveRetornarNotFound()
    {
        // Arrange
        const int userId = 99;
        _mediator.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<OneOf<DeleteUserResponse, NotFoundError>>(new NotFoundError("User not found")));

        // Act
        var result = await _controller.DeleteUser(userId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }
}
