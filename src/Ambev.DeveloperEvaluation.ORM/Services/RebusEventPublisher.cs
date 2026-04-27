using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.ORM.Services;

/// <summary>
/// Implementação de infraestrutura de IEventPublisher usando Rebus como barramento de mensagens.
/// Pertence à camada de Infraestrutura (ORM) pois encapsula uma dependência externa (Rebus).
/// A camada Application depende apenas da abstração IEventPublisher definida no Domain.
/// </summary>
public class RebusEventPublisher : IEventPublisher
{
    private readonly IBus _bus;

    public RebusEventPublisher(IBus bus)
    {
        _bus = bus;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class
    {
        await _bus.Publish(@event);
    }
}
