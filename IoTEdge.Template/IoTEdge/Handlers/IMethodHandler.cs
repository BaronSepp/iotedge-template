using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.IoTEdge.Handlers;

/// <summary>
/// Interface for handling C2D method calls.
/// </summary>
public interface IMethodHandler
{
    /// <summary>
    /// The default delegate that applies for all method endpoints.
    /// If a delegate is already associated with the method, it
    /// will be called, else the default delegate will be called.
    /// </summary>
    /// <param name="method">Class with details about method.</param>
    /// <param name="userContext">userContext</param>
    /// <returns><inheritdoc cref="Task{MessageResponse}"/></returns>
    public Task<MethodResponse> Default(MethodRequest method, object userContext);
}
