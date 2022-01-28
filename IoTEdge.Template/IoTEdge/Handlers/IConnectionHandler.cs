using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.Module.IoTEdge.Handlers;

public interface IConnectionHandler
{
    void OnConnectionChange(ConnectionStatus status, ConnectionStatusChangeReason reason);
}
