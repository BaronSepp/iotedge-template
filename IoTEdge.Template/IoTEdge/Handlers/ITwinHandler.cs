using Microsoft.Azure.Devices.Shared;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public interface ITwinHandler
{
    Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext);
}
