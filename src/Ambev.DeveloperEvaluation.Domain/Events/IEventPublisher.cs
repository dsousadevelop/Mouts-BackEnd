namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Abstração para publicação de eventos de domínio.
/// Implementada na camada de Infraestrutura, injeta Rebus sem violar a arquitetura.
/// </summary>
public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
        where T : class;
}
