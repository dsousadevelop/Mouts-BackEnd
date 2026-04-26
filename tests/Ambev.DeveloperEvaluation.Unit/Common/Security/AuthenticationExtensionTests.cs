using Ambev.DeveloperEvaluation.Common.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Common.Security;

public class AuthenticationExtensionTests
{
    [Fact(DisplayName = "AddJwtAuthentication should register IJwtTokenGenerator")]
    public void AddJwtAuthentication_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:SecretKey"].Returns("super_secret_key_123_abc_456_def_789");
        services.AddSingleton(configuration);

        // Act
        services.AddJwtAuthentication(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var jwtGenerator = serviceProvider.GetService<IJwtTokenGenerator>();
        jwtGenerator.Should().NotBeNull();
        jwtGenerator.Should().BeOfType<JwtTokenGenerator>();
    }

    [Fact(DisplayName = "AddJwtAuthentication should throw exception if secret key is missing")]
    public void AddJwtAuthentication_ShouldThrowException_WhenSecretKeyIsMissing()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        configuration["Jwt:SecretKey"].Returns((string?)null);

        // Act
        var action = () => services.AddJwtAuthentication(configuration);

        // Assert
        action.Should().Throw<ArgumentException>();
    }
}
