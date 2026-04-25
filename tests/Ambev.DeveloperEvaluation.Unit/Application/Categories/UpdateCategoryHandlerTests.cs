using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Categories;

public class UpdateCategoryHandlerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly UpdateCategoryHandler _handler;

    public UpdateCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateCategoryHandler(_categoryRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid data When updating category Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Given
        var categoryId = 1;
        var categoryDto = new CategoryDto(categoryId, "Updated Description");
        var command = new UpdateCategoryCommand(categoryDto);
        var existingCategory = new Category(categoryId, "Old Description");

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(existingCategory);
        _categoryRepository.GetByDescriptionAsync("Updated Description", Arg.Any<CancellationToken>()).Returns((Category?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        existingCategory.Description.Should().Be("Updated Description");
        await _categoryRepository.Received(1).UpdateAsync(existingCategory, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given nonexistent id When updating category Then returns resource not found error")]
    public async Task Handle_NonExistentCategory_ReturnsResourceNotFoundError()
    {
        // Given
        var categoryId = 1;
        var categoryDto = new CategoryDto(categoryId, "Updated Description");
        var command = new UpdateCategoryCommand(categoryDto);

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns((Category?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(categoryId.ToString());
    }

    [Fact(DisplayName = "Given existing description When updating category Then returns validation error")]
    public async Task Handle_DuplicateDescription_ReturnsValidationError()
    {
        // Given
        var categoryId = 1;
        var categoryDto = new CategoryDto(categoryId, "Existing Description");
        var command = new UpdateCategoryCommand(categoryDto);
        var existingCategory = new Category(categoryId, "Old Description");
        var otherCategory = new Category(2, "Existing Description");

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(existingCategory);
        _categoryRepository.GetByDescriptionAsync("Existing Description", Arg.Any<CancellationToken>()).Returns(otherCategory);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT2.Should().BeTrue();
        result.AsT2.Detail.Should().Contain("already exists");
    }
}

