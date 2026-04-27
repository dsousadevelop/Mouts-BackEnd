using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.MicroServices.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Bus;
using Rebus.Config;

var builder = Host.CreateApplicationBuilder(args);

// Consumer com fila dedicada — NÃO usar onCreated pois o IBus ainda não está
// resolvido pelo MS DI naquele ponto, causando erro no Injectionist do Rebus.
var rabbitMqConnection = builder.Configuration.GetConnectionString("RabbitMQ")
    ?? "amqp://guest:guest@127.0.0.1:5672";

builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq(rabbitMqConnection, "email-queue"))
);

// Registrar o Handler de mensagens
builder.Services.AddRebusHandler<EmailMessageHandler>();

// IHostedService que faz o Subscribe APÓS o host estar completamente inicializado
builder.Services.AddHostedService<RebusSubscriptionService>();

var host = builder.Build();
await host.RunAsync();


/// <summary>
/// Realiza a assinatura dos eventos no RabbitMQ após o host estar completamente inicializado.
/// Garante que o IBus do Rebus já está resolvido pelo container MS DI.
/// </summary>
internal sealed class RebusSubscriptionService : IHostedService
{
    private readonly IBus _bus;

    public RebusSubscriptionService(IBus bus)
    {
        _bus = bus;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _bus.Subscribe<CartCreatedEvent>();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
