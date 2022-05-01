using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoT.TwinHandlers;

/// <summary>
/// Interface for handling module twin updates.
/// </summary>
public interface ITwinHandler
{
    /// <summary>
    /// The callback that will be called whenever the 
    /// client receives a state update from the service.
    /// </summary>
    /// <param name="desiredProperties">Properties that were contained in the update that was received from the service</param>
    /// <param name="userContext">Context object passed in when the callback was registered</param>
    /// <returns><inheritdoc cref="Task"/></returns>
    /// <inheritdoc cref="DesiredPropertyUpdateCallback"/>
    Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext);
}
