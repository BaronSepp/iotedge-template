using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;

namespace IoTEdge.Template.Module.IoTEdge.Handlers;
public interface IMethodHandler
{
    public Task<MethodResponse> Default(MethodRequest method, object userContext);
}
