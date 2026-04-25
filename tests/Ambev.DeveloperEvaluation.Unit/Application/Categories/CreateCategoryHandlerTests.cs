using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Categories;

public class CreateCategoryHandlerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateCategoryHandler(_categoryRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid data When creating category Then returns created category")]
    public async Task Handle_ValidRequest_ReturnsCategoryDto()
    {
        // Given
        var categoryDto = new CategoryDto(null, "Test Category");
        var command = new CreateCategoryCommand(categoryDto);
        var category = new Category(1, "Test Category");

        _categoryRepository.GetByDescriptionAsync("Test Category", Arg.Any<CancellationToken>()).Returns((Category?)null);
        _mapper.Map<Category>(Arg.Any<CategoryDto>()).Returns(category);
        _categoryRepository.CreateAsync(category, Arg.Any<CancellationToken>()).Returns(category);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(1);
        result.AsT0.Description.Should().Be("Test Category");
        await _categoryRepository.Received(1).CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given existing description When creating category Then returns validation error")]
    public async Task Handle_ExistingDescription_ReturnsValidationError()
    {
        // Given
        var categoryDto = new CategoryDto(null, "Test Category");
        var command = new CreateCategoryCommand(categoryDto);
        var existingCategory = new Category(1, "Test Category");

        _categoryRepository.GetByDescriptionAsync("Test Category", Arg.Any<CancellationToken>()).Returns(existingCategory);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain("already exists");
    }
}

