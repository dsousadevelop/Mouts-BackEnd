using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Middleware;

public class ValidationExceptionMiddlewareTests
{
    private readonly RequestDelegate _next;
    private readonly ValidationExceptionMiddleware _middleware;
    private readonly DefaultHttpContext _context;

    public ValidationExceptionMiddlewareTests()
    {
        _next = Substitute.For<RequestDelegate>();
        _middleware = new ValidationExceptionMiddleware(_next);
        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();
    }

    [Fact]
    public async Task InvokeAsync_WhenNoException_ShouldCallNext()
    {
        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        await _next.Received(1).Invoke(_context);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationException_ShouldReturnBadRequest()
    {
        // Arrange
        var errors = new List<ValidationFailure>
        {
            new("PropertyName", "ErrorMessage") { ErrorCode = "400" }
        };
        var exception = new ValidationException(errors);
        _next.When(x => x.Invoke(Arg.Any<HttpContext>())).Do(x => throw exception);

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        _context.Response.ContentType.Should().Be("application/json");

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(_context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var response = JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        response.Should().NotBeNull();
        response!.Success.Should().BeFalse();
        response.Message.Should().Be("Validation Failed");
        response.Errors.Should().NotBeNull();
        response.Errors.Should().HaveCount(1);

        var error = response.Errors!.First();
        error.Description.Should().Be("ErrorMessage");
        error.Code.Should().Be("400");
    }
}
