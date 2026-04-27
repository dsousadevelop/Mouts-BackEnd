using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Categories;

public class DeleteCategoryHandlerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly DeleteCategoryHandler _handler;

    public DeleteCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _handler = new DeleteCategoryHandler(_categoryRepository);
    }

    [Fact(DisplayName = "Dado um ID válido, ao deletar a categoria, retorna sucesso")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        const int categoryId = 1;
        var command = new DeleteCategoryCommand(categoryId);
        var category = new Category(categoryId, "Test Category");

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
        _categoryRepository.DeleteAsync(categoryId, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        await _categoryRepository.Received(1).DeleteAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Dado um ID inexistente, ao deletar a categoria, retorna erro de recurso não encontrado")]
    public async Task Handle_NonExistentCategory_ReturnsResourceNotFoundError()
    {
        // Given
        const int categoryId = 1;
        var command = new DeleteCategoryCommand(categoryId);

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns((Category?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(categoryId.ToString());
    }
}
