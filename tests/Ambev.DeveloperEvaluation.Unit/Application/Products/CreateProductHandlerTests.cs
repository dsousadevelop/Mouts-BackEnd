using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.DTOs;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Products.Fakers;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cacheService = Substitute.For<ICacheService>();
        _handler = new CreateProductHandler(_productRepository, _mapper, _cacheService);
    }

    [Fact(DisplayName = "Given valid product data When creating product Then returns product dto")]
    public async Task Handle_ValidRequest_ReturnsProductDto()
    {
        // Given
        var productDto = ProdutoFaker.GerarProductDto(id: null);
        var command = new CreateProductCommand(productDto);
        var product = ProdutoFaker.GerarProdutoValido(id: 1);
        var expectedDto = ProdutoFaker.GerarProductDto(id: 1);

        _mapper.Map<Product>(command.ProductDto).Returns(product);
        _productRepository.CreateAsync(product, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<ProductDto>(product).Returns(expectedDto);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(1);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _cacheService.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }
}
