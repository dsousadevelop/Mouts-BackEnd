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
/// Testes unitários para o Handler de atualização de produtos.
/// Verifica se o produto existe, se os dados são atualizados e se o cache é limpo.
/// </summary>
public class UpdateProductHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _cache = Substitute.For<ICacheService>();
        _handler = new UpdateProductHandler(_repository, _mapper, _cache);
    }

    [Fact(DisplayName = "Deve atualizar o produto com sucesso quando ele existir")]
    public async Task Handle_ProdutoExistente_DeveAtualizarELimparCache()
    {
        // Arrange
        var productDto = ProdutoFaker.GerarProductDto(id: 1);
        var command = new UpdateProductCommand(productDto);
        var existingProduct = ProdutoFaker.GerarProdutoValido(id: 1);

        _repository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(existingProduct);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue(); // Success
        await _repository.Received(1).UpdateAsync(existingProduct, Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Deve retornar erro de Não Encontrado quando o produto não existir")]
    public async Task Handle_ProdutoNaoExistente_DeveRetornarNotFoundError()
    {
        // Arrange
        var productDto = ProdutoFaker.GerarProductDto(id: 99);
        var command = new UpdateProductCommand(productDto);

        _repository.GetByIdAsync(99, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue(); // NotFoundError
        result.AsT1.Detail.Should().Contain("not found");
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}
