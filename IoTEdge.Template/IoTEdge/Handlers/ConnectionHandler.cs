using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;

namespace IoTEdge.Template.IoTEdge.Handlers;
public sealed class ConnectionHandler : IConnectionHandler
{
    private readonly ILogger<ConnectionHandler> _logger;

    // Metrics
    private readonly Counter ConnectionChangeCounter =
        Metrics.CreateCounter("connection_changes", "Amount of times the connection has changed");

    public ConnectionHandler(ILogger<ConnectionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void OnConnectionChange(ConnectionStatus status, ConnectionStatusChangeReason reason)
    {
        ConnectionChangeCounter.Inc();
        _logger.LogInformation("Connection changed to status {status} for reason {reason}.", status, reason);
    }
}
