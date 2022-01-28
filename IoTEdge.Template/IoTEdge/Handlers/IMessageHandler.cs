using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public interface IMessageHandler
{
    public Task<MessageResponse> Default(Message message, object userContext);
}
