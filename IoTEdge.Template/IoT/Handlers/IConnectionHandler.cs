using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.IoT.Handlers;

/// <summary>
/// Interface for handling connection status changes.
/// </summary>
public interface IConnectionHandler
{
    /// <summary>
    /// The delegate that will be called when the connection status changes.
    /// </summary>
    /// <remarks>
    /// This callback will never be called if the client is
    /// configured to use HTTP as that protocol is stateless.
    /// </remarks>
    /// <param name="status">The updated connection status.</param>
    /// <param name="reason">The reason for the connection status change.</param>
    void OnConnectionChange(ConnectionStatus status, ConnectionStatusChangeReason reason);
}
