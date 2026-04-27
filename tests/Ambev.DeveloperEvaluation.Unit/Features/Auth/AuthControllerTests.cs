using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Features.Auth;

public class AuthControllerTests : ControllerTestsBase
{
    private readonly AuthController _controller;
    private readonly Faker _faker;

    public AuthControllerTests()
    {
        _controller = new AuthController(Mediator, Mapper);
        _faker = new Faker();
    }

    [Fact(DisplayName = "AuthenticateUser deve retornar Ok para requisição válida")]
    public async Task AuthenticateUser_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new AuthenticateUserRequest
        {
            Email = _faker.Internet.Email(),
            Password = "Password123!"
        };

        var command = new AuthenticateUserCommand { Email = request.Email, Password = request.Password };
        var result = new AuthenticateUserResult { Token = "token" };
        var response = new AuthenticateUserResponse { Token = "token" };

        Mapper.Map<AuthenticateUserCommand>(request).Returns(command);
        Mediator.Send(Arg.Any<AuthenticateUserCommand>(), Arg.Any<CancellationToken>())
            .Returns(result);
        Mapper.Map<AuthenticateUserResponse>(result).Returns(response);

        // Act
        var actionResult = await _controller.AuthenticateUser(request, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<OkObjectResult>();
        var okResult = actionResult as OkObjectResult;
        var apiResponse = okResult?.Value as ApiResponseWithData<AuthenticateUserResponse>;
        apiResponse.Should().NotBeNull();
        apiResponse!.Data!.Token.Should().Be("token");
    }
}
