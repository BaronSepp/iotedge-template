using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public sealed class MethodHandler : IMethodHandler
{
    private readonly ILogger<MethodHandler> _logger;

    public MethodHandler(ILogger<MethodHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<MethodResponse> Default(MethodRequest method, object userContext)
    {
        _logger.LogInformation("Unhandled method received.", method);

        var response = new MethodResponse(JsonSerializer.SerializeToUtf8Bytes("may be short and stout"), 418);
        return Task.FromResult(response);
    }
}
