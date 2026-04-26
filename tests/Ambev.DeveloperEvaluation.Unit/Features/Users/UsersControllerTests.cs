using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Unit.Features.Users;

public class UsersControllerTests : ControllerTestsBase
{
    private readonly UsersController _controller;
    private readonly Faker _faker;

    public UsersControllerTests()
    {
        _controller = new UsersController(Mediator, Mapper);
        _faker = new Faker();
    }

    [Fact]
    public async Task CreateUser_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Username = _faker.Internet.UserName(),
            Password = "Password123!",
            Email = _faker.Internet.Email(),
            Phone = "+5511999999999",
            Status = UserStatus.Active,
            Role = UserRole.Customer
        };

        var command = new CreateUserCommand { Username = request.Username };
        var result = new CreateUserResult { Id = 1 };
        var response = new CreateUserResponse { Id = 1 };

        Mapper.Map<CreateUserCommand>(request).Returns(command);
        Mediator.Send(Arg.Any<CreateUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(result);
        Mapper.Map<CreateUserResponse>(result).Returns(response);

        // Act
        var actionResult = await _controller.CreateUser(request, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<CreatedResult>();
        var createdResult = actionResult as CreatedResult;
        var apiResponse = createdResult?.Value as ApiResponseWithData<CreateUserResponse>;
        apiResponse.Should().NotBeNull();
        apiResponse!.Data!.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetUser_ExistingId_ReturnsOk()
    {
        // Arrange
        var id = _faker.Random.Int(1, 100);
        var command = new GetUserCommand(id);
        var result = new GetUserResult { Id = id };
        var response = new GetUserResponse { Id = id };

        Mapper.Map<GetUserCommand>(id).Returns(command);
        Mediator.Send(Arg.Any<GetUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(result);
        Mapper.Map<GetUserResponse>(result).Returns(response);

        // Act
        var actionResult = await _controller.GetUser(id, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponseWithData<GetUserResponse>;
        apiResponse.Should().NotBeNull();
        apiResponse!.Data!.Id.Should().Be(id);
    }

    [Fact]
    public async Task DeleteUser_ValidId_ReturnsOk()
    {
        // Arrange
        var id = _faker.Random.Int(1, 100);
        var command = new DeleteUserCommand(id);

        Mapper.Map<DeleteUserCommand>(id).Returns(command);
        Mediator.Send(Arg.Any<DeleteUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(new DeleteUserResponse());

        // Act
        var actionResult = await _controller.DeleteUser(id, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
    }
}
