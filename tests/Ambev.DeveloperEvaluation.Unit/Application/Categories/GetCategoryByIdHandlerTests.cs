using Ambev.DeveloperEvaluation.Application.Common.Errors;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Categories;

public class GetCategoryByIdHandlerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly GetCategoryByIdHandler _handler;

    public GetCategoryByIdHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetCategoryByIdHandler(_categoryRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid id When category exists Then returns category details")]
    public async Task Handle_ExistingCategory_ReturnsCategoryDto()
    {
        // Given
        const int categoryId = 1;
        var query = new GetCategoryByIdQuery(categoryId);
        var category = new Category(categoryId, "Test Category");
        var categoryDto = new CategoryDto(categoryId, "Test Category");

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns(category);
        _mapper.Map<CategoryDto>(category).Returns(categoryDto);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeEquivalentTo(categoryDto);
    }

    [Fact(DisplayName = "Given nonexistent id When category does not exist Then returns resource not found error")]
    public async Task Handle_NonExistentCategory_ReturnsResourceNotFoundError()
    {
        // Given
        const int categoryId = 1;
        var query = new GetCategoryByIdQuery(categoryId);

        _categoryRepository.GetByIdAsync(categoryId, Arg.Any<CancellationToken>()).Returns((Category?)null);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.IsT1.Should().BeTrue();
        result.AsT1.Detail.Should().Contain(categoryId.ToString());
    }
}
