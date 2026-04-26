using Ambev.DeveloperEvaluation.Application.Common.Errors;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Common.Errors
{
    public class ApplicationErrorsTests
    {
        [Fact(DisplayName = "AuthenticationError deve definir as propriedades corretas no construtor")]
        public void AuthenticationError_Constructor_SetsCorrectProperties()
        {
            // Arrange
            var detail = "Invalid token provided";

            // Act
            var error = new AuthenticationError(detail);

            // Assert
            error.Type.Should().Be("AuthenticationError");
            error.Error.Should().Be("Invalid authentication token");
            error.Detail.Should().Be(detail);
        }

        [Fact(DisplayName = "ValidationError deve definir as propriedades corretas no construtor")]
        public void ValidationError_Constructor_SetsCorrectProperties()
        {
            // Arrange
            var detail = "Field is required";

            // Act
            var error = new ValidationError(detail);

            // Assert
            error.Type.Should().Be("ValidationError");
            error.Error.Should().Be("Invalid input data");
            error.Detail.Should().Be(detail);
        }

        [Fact(DisplayName = "NotFoundError deve definir as propriedades corretas no construtor")]
        public void NotFoundError_Constructor_SetsCorrectProperties()
        {
            // Arrange
            var detail = "User not found";

            // Act
            var error = new NotFoundError(detail);

            // Assert
            error.Type.Should().Be("NotFound");
            error.Error.Should().Be("Resource not found");
            error.Detail.Should().Be(detail);
        }

        [Fact(DisplayName = "ResourceNotFoundError deve definir as propriedades corretas no construtor")]
        public void ResourceNotFoundError_Constructor_SetsCorrectProperties()
        {
            // Arrange
            var detail = "Product not found";

            // Act
            var error = new ResourceNotFoundError(detail);

            // Assert
            error.Type.Should().Be("ResourceNotFound");
            error.Error.Should().Be("Resource not found");
            error.Detail.Should().Be(detail);
        }
    }
}
