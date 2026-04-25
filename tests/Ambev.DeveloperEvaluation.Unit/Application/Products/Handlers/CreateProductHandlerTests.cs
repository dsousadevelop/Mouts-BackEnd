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

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Handlers;

/// <summary>
/// Testes unitários para o Handler de criação de produtos.
/// Valida o fluxo de mapeamento, persistência e limpeza de cache.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cache = Substitute.For<ICacheService>();
        _handler = new CreateProductHandler(_repository, _mapper, _cache);
    }

    [Fact(DisplayName = "Deve criar um produto com sucesso e invalidar o cache da listagem")]
    public async Task Handle_DadoComandoValido_DeveRetornarProdutoDtoELimparCache()
    {
        // Arrange
        var productDto = ProdutoFaker.GerarProductDto(id: null);
        var command = new CreateProductCommand(productDto);
        var product = ProdutoFaker.GerarProdutoValido(id: 1);
        var expectedDto = ProdutoFaker.GerarProductDto(id: 1);

        _mapper.Map<Product>(command.ProductDto).Returns(product);
        _repository.CreateAsync(product, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<ProductDto>(product).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(1);

        await _repository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Deve mapear corretamente o comando para a entidade de domínio")]
    public async Task Handle_ValidacaoMapeamento_DeveChamarMapperComDadosCorretos()
    {
        // Arrange
        var productDto = ProdutoFaker.GerarProductDto();
        var command = new CreateProductCommand(productDto);
        var product = ProdutoFaker.GerarProdutoValido();

        _mapper.Map<Product>(command.ProductDto).Returns(product);
        _repository.CreateAsync(product, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Product>(command.ProductDto);
    }
}
