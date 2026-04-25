using Ambev.DeveloperEvaluation.Application.Features.Categories.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Categories.Queries;
using Ambev.DeveloperEvaluation.Application.Features.Categories.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.CreateCategory;
using Ambev.DeveloperEvaluation.WebApi.Features.Categories.ListCategories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using OneOf;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Common.Errors;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Categories;

public class CategoryControllerTests
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();
        _controller = new CategoryController(_mediator, _mapper);
    }

    [Fact(DisplayName = "List deve retornar 200 Ok quando houver categorias")]
    public async Task List_QuandoExistiremCategorias_DeveRetornarOk()
    {
        // Arrange
        var categories = new List<CategoryDto> { new CategoryDto(1, "Electronics") };
        _mediator.Send(Arg.Any<GetCategoryListQuery>(), Arg.Any<CancellationToken>())
            .Returns(categories);
        
        _mapper.Map<IEnumerable<ListCategoriesResponse>>(categories)
            .Returns(new List<ListCategoriesResponse> { new ListCategoriesResponse { Id = 1, Description = "Electronics" } });

        // Act
        var result = await _controller.List(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact(DisplayName = "Post deve retornar 201 Created")]
    public async Task Post_ComDadosValidos_DeveRetornarCreated()
    {
        // Arrange
        var request = new CreateCategoryRequest { Description = "New Category" };
        var categoryDto = new CategoryDto(1, "New Category");
        var command = new CreateCategoryCommand(categoryDto);

        _mapper.Map<CreateCategoryCommand>(request).Returns(command);
        _mediator.Send(command, Arg.Any<CancellationToken>())
            .Returns((OneOf<CategoryDto, ValidationError>)categoryDto);
        _mapper.Map<CreateCategoryResponse>(categoryDto).Returns(new CreateCategoryResponse { Id = 1 });

        // Act
        var result = await _controller.Post(request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>().Which.StatusCode.Should().Be(201);
    }
}
