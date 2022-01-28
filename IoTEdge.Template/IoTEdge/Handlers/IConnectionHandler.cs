using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.IoTEdge.Handlers;

public interface IConnectionHandler
{
    void OnConnectionChange(ConnectionStatus status, ConnectionStatusChangeReason reason);
}
