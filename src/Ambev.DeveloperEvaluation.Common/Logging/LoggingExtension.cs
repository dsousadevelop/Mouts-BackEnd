using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Templates;
using System.Diagnostics;

namespace Ambev.DeveloperEvaluation.Common.Logging;

/// <summary> Add default Logging configuration to project. This configuration supports Serilog logs with DataDog compatible output.</summary>
public static class LoggingExtension
{
    /// <summary>
    /// The destructuring options builder configured with default destructurers and a custom DbUpdateExceptionDestructurer.
    /// </summary>
    static readonly DestructuringOptionsBuilder _destructuringOptionsBuilder = new DestructuringOptionsBuilder()
        .WithDefaultDestructurers()
        .WithDestructurers([new DbUpdateExceptionDestructurer()]);

    /// <summary>
    /// A filter predicate to exclude log events with specific criteria.
    /// </summary>
    static readonly Func<LogEvent, bool> _filterPredicate = exclusionPredicate =>
    {
        if (exclusionPredicate.Level != LogEventLevel.Information) return false;

        exclusionPredicate.Properties.TryGetValue("StatusCode", out var statusCode);
        exclusionPredicate.Properties.TryGetValue("Path", out var path);

        var excludeByStatusCode = statusCode == null || statusCode.ToString().Equals("200");
        var excludeByPath = path?.ToString().Contains("/health") ?? false;

        return excludeByStatusCode && excludeByPath;
    };

    /// <summary>
    /// This method configures the logging with commonly used features for DataDog integration.
    /// </summary>
    /// <param name="builder">The <see cref="IHostApplicationBuilder" /> to add services to.</param>
    /// <returns>The <see cref="IHostApplicationBuilder"/> that can be used to further configure the services.</returns>
    public static TBuilder AddDefaultLogging<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails(_destructuringOptionsBuilder)
            .Filter.ByExcluding(_filterPredicate)
            .WriteTo.Conditional(_ => Debugger.IsAttached, 
                wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}", theme: SystemConsoleTheme.Colored)
                        .Enrich.WithProperty("DebuggerAttached", true))
            .WriteTo.Conditional(_ => !Debugger.IsAttached,
                wt => wt.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}"))
            .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? "http://localhost:5341")
            .CreateLogger();

        builder.Services.AddSerilog();

        return builder;
    }

    /// <summary>Adds middleware for log reporting.</summary>
    /// <param name="app">The <see cref="IHost"/> instance this method extends.</param>
    /// <returns>The <see cref="IHost"/> for log reporting.</returns>
    public static IHost UseDefaultLogging(this IHost app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Logger>>();
        var env = app.Services.GetRequiredService<IHostEnvironment>();

        var mode = Debugger.IsAttached ? "Debug" : "Release";
        logger.LogInformation("Logging enabled for '{Application}' on '{Environment}' - Mode: {Mode}", env.ApplicationName, env.EnvironmentName, mode);
        return app;
    }
}
