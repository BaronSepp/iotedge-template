using Microsoft.Azure.Devices.Shared;
using System.Threading.Tasks;

namespace IoTEdge.Template.Module.IoTEdge.Handlers;
public interface ITwinHandler
{
    Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext);
}
