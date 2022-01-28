using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public sealed class TwinHandler : ITwinHandler
{
    private readonly ILogger<TwinHandler> _logger;

    public TwinHandler(ILogger<TwinHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
    {
        _logger.LogInformation("Incoming desired properties: {properties}", desiredProperties.ToJson());
        return Task.CompletedTask;
    }
}
