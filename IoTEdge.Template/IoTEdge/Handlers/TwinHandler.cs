using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace IoTEdge.Template.IoTEdge.Handlers;

/// <summary>
/// Implementation for handling module twin updates.
/// </summary>
public sealed class TwinHandler : ITwinHandler
{
    private readonly ILogger<TwinHandler> _logger;

    // Metrics
    private readonly Counter TwinUpdateCounter =
        Metrics.CreateCounter("twin_updates_received", "Amount of twin updates received");

    /// <summary>
    /// Public <see cref="TwinHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
    public TwinHandler(ILogger<TwinHandler> logger)
    {
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

        TwinUpdateCounter.Inc();
    }
}
