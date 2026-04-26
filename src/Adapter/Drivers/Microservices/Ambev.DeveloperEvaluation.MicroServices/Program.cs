using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.MicroServices.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;

var builder = Host.CreateApplicationBuilder(args);

// Configuração do Rebus
builder.Services.AddRebus(configure => configure
    .Logging(l => l.MicrosoftExtensionsLogging(builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>()))
    .Transport(t => t.UseRabbitMq("amqp://guest:guest@localhost:5672", "email-queue"))
    .Routing(r => r.TypeBased().Map<CartCreatedEvent>("email-queue"))
);

// Registrar o Handler
builder.Services.AutoRegisterHandlersFromAssemblyOf<EmailMessageHandler>();

var host = builder.Build();

// Subscrever ao evento
var bus = host.Services.GetRequiredService<IBus>();
await bus.Subscribe<CartCreatedEvent>();

await host.RunAsync();
