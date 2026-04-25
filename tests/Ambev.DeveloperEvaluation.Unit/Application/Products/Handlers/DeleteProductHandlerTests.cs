using Ambev.DeveloperEvaluation.Application.Common.Interfaces;
using Ambev.DeveloperEvaluation.Application.Features.Products.Commands;
using Ambev.DeveloperEvaluation.Application.Features.Products.Handlers;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.Handlers;

/// <summary>
/// Testes unitários para o Handler de exclusão de produtos.
/// Verifica sucesso na exclusão e comportamento em caso de ID inexistente.
/// </summary>
public class DeleteProductHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cache;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _cache = Substitute.For<ICacheService>();
        _handler = new DeleteProductHandler(_repository, _cache);
    }

    [Fact(DisplayName = "Deve excluir o produto e limpar o cache quando o ID for válido")]
    public async Task Handle_IdValido_DeveExcluirELimparCache()
    {
        // Arrange
        int idParaExcluir = 1;
        var command = new DeleteProductCommand(idParaExcluir);
        _repository.DeleteAsync(idParaExcluir, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT0.Should().BeTrue(); // Success
        await _repository.Received(1).DeleteAsync(idParaExcluir, Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync("Products_List", Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Deve retornar erro de Não Encontrado quando tentar excluir ID inexistente")]
    public async Task Handle_IdInexistente_DeveRetornarNotFoundError()
    {
        // Arrange
        int idInexistente = 999;
        var command = new DeleteProductCommand(idInexistente);
        _repository.DeleteAsync(idInexistente, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsT1.Should().BeTrue(); // NotFoundError
        await _cache.DidNotReceive().RemoveAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}
