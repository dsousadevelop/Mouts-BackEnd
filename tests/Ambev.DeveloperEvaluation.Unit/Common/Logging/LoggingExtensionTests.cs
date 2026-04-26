using Ambev.DeveloperEvaluation.Common.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Common.Logging;

public class LoggingExtensionTests
{
    [Fact]
    public void AddDefaultLogging_ShouldRegisterLoggingServices()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        builder.AddDefaultLogging();
        var app = builder.Build();

        // Assert
        var loggerProvider = app.Services.GetService<ILoggerProvider>();
        // Serilog registers its own provider
        loggerProvider.Should().NotBeNull();
    }

    [Fact]
    public void UseDefaultLogging_ShouldNotThrowException()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        builder.AddDefaultLogging();
        var app = builder.Build();

        // Act
        var act = () => app.UseDefaultLogging();

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(LogEventLevel.Information, "200", "/health", true)] // Excluded (Noise)
    [InlineData(LogEventLevel.Information, "200", "/api/users", false)] // Kept
    [InlineData(LogEventLevel.Warning, null, null, false)] // Kept
    [InlineData(LogEventLevel.Error, null, null, false)] // Kept
    public void FilterPredicate_ShouldCorrectlyIdentifyEventsToExclude(LogEventLevel level, string? statusCode, string? path, bool expectedExclusion)
    {
        // Arrange
        var field = typeof(LoggingExtension).GetField("_filterPredicate", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        var filterPredicate = (Func<LogEvent, bool>)field!.GetValue(null)!;

        var properties = new List<LogEventProperty>();
        if (statusCode != null)
        {
            if (int.TryParse(statusCode, out var intStatus))
                properties.Add(new LogEventProperty("StatusCode", new ScalarValue(intStatus)));
            else
                properties.Add(new LogEventProperty("StatusCode", new ScalarValue(statusCode)));
        }
        if (path != null) properties.Add(new LogEventProperty("Path", new ScalarValue(path)));

        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            level,
            null,
            new MessageTemplate(new List<MessageTemplateToken>()),
            properties
        );

        // Act
        var result = filterPredicate(logEvent);

        // Assert
        result.Should().Be(expectedExclusion);
    }
}
