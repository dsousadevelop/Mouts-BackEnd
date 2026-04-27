using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Activation;
using Ambev.DeveloperEvaluation.ORM;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<Microsoft.EntityFrameworkCore.DbContext>(
            provider => provider.GetRequiredService<DefaultContext>());

        // WebAPI é SOMENTE publisher — usa cliente unidirecional sem fila própria.
        // Registramos o IBus manualmente como Singleton para evitar conflitos do Injectionist
        // e o IHostedService padrão do AddRebus, que tenta iniciar o Bus cedo demais.
        var rabbitMqConnection = builder.Configuration.GetConnectionString("RabbitMQ");
        
        builder.Services.AddSingleton<Rebus.Bus.IBus>(provider =>
        {
            var bus = Configure.With(new BuiltinHandlerActivator())
                .Transport(t => t.UseRabbitMqAsOneWayClient(rabbitMqConnection))
                .Start();
            return bus;
        });
    }
}
