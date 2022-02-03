using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public sealed class MethodHandler : IMethodHandler
{
    private readonly ILogger<MethodHandler> _logger;

    // Metrics
    private readonly Counter UnhandledMethodCounter =
        Metrics.CreateCounter("unhandled_method_counter", "Amount of unhandled methods received");

    public MethodHandler(ILogger<MethodHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<MethodResponse> Default(MethodRequest method, object userContext)
    {
        UnhandledMethodCounter.Inc();
        _logger.LogInformation("Unhandled method '{Method}' received with data '{Data}'.", method.Name, method.DataAsJson);

        var response = new MethodResponse(JsonSerializer.SerializeToUtf8Bytes("short and stout"), 418);
        return Task.FromResult(response);
    }
}
