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

public class GetCategoryListHandlerTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly GetCategoryListHandler _handler;

    public GetCategoryListHandlerTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetCategoryListHandler(_categoryRepository, _mapper);
    }

    [Fact(DisplayName = "When listing categories Then returns all categories")]
    public async Task Handle_ValidRequest_ReturnsCategoryList()
    {
        // Given
        var query = new GetCategoryListQuery();
        var categories = new List<Category>
        {
            new Category(1, "C1"),
            new Category(2, "C2")
        };

        var categoryDtos = new List<CategoryDto>
        {
            new(1, "C1"),
            new(2, "C2")
        };

        _categoryRepository.GetListAllAsync(Arg.Any<CancellationToken>()).Returns(categories);
        _mapper.Map<List<CategoryDto>>(categories).Returns(categoryDtos);

        // When
        var result = await _handler.Handle(query, CancellationToken.None);

        // Then
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(categoryDtos);
        await _categoryRepository.Received(1).GetListAllAsync(Arg.Any<CancellationToken>());
    }
}
