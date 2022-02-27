using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace IoTEdge.Template.IoTEdge.Handlers;

/// <summary>
/// Implementation for handling C2D method calls.
/// </summary>
public sealed class MethodHandler : IMethodHandler
{
    private readonly Counter _unhandledMethodCounter;
    private readonly ILogger<MethodHandler> _logger;

    /// <summary>
    /// Public <see cref="MethodHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
    public MethodHandler(ILogger<MethodHandler> logger)
    {
        _unhandledMethodCounter = Metrics.CreateCounter("unhandled_method_counter", "Amount of unhandled methods received");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc cref="IMethodHandler.Default(MethodRequest, object)"/>
    public Task<MethodResponse> Default(MethodRequest method, object userContext)
    {
        _unhandledMethodCounter.Inc();
        _logger.LogInformation("Unhandled method '{Method}' received with data '{Data}'.", method.Name, method.DataAsJson);

        var response = new MethodResponse(JsonSerializer.SerializeToUtf8Bytes("short and stout"), 418);
        return Task.FromResult(response);
    }
}
