using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoT.TwinHandlers;

/// <summary>
/// Implementation for handling module twin updates.
/// </summary>
public sealed class TwinHandler : ITwinHandler
{
    private readonly Counter _twinUpdateCounter;
    private readonly ILogger<TwinHandler> _logger;

    /// <summary>
    /// Public <see cref="TwinHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
    public TwinHandler(ILogger<TwinHandler> logger)
    {
        _twinUpdateCounter = Metrics.CreateCounter("twin_updates_received", "Amount of twin updates received");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc cref="ITwinHandler.OnDesiredPropertiesUpdate(TwinCollection, object)"/>
    public async Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
    {
        desiredProperties.ClearMetadata();
        _logger.LogInformation("Incoming desired properties: {properties}", desiredProperties.ToJson());

        if (userContext is IModuleClient moduleClient)
        {
            await moduleClient.UpdateReportedPropertiesAsync(desiredProperties);
        }

        _twinUpdateCounter.Inc();
    }
}
