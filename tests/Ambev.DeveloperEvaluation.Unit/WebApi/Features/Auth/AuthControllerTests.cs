using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Auth;

public class AuthControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly AuthController _controller;
    private readonly Faker _faker;

    public AuthControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new AuthController(_mediator, _mapper);
        _faker = new Faker();
    }

    [Fact(DisplayName = "AuthenticateUser deve retornar 200 Ok quando credenciais são válidas")]
    public async Task AuthenticateUser_ComCredenciaisValidas_DeveRetornarOk()
    {
        // Arrange
        var request = new AuthenticateUserRequest
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        var command = new AuthenticateUserCommand { Email = request.Email, Password = request.Password };
        var result = new AuthenticateUserResult
        {
            Token = _faker.Random.AlphaNumeric(32),
            Email = request.Email,
            Name = _faker.Person.FullName,
            Role = "Admin"
        };

        var response = new AuthenticateUserResponse
        {
            Token = result.Token
        };

        _mapper.Map<AuthenticateUserCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>()).Returns(result);
        _mapper.Map<AuthenticateUserResponse>(result).Returns(response);

        // Act
        var actionResult = await _controller.AuthenticateUser(request, CancellationToken.None);

        // Assert
        var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var apiResponse = okResult.Value.Should().BeOfType<ApiResponseWithData<AuthenticateUserResponse>>().Subject;
        
        apiResponse.Success.Should().BeTrue();
        apiResponse.Data!.Token.Should().Be(result.Token);
        await _mediator.Received(1).Send(Arg.Any<AuthenticateUserCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "AuthenticateUser deve retornar 400 BadRequest quando request é inválido")]
    public async Task AuthenticateUser_ComRequestInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var request = new AuthenticateUserRequest
        {
            Email = "", // Inválido
            Password = ""
        };

        // Act
        var actionResult = await _controller.AuthenticateUser(request, CancellationToken.None);

        // Assert
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
}
