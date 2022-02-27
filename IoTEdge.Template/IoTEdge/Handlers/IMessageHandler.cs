using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;

/// <summary>
/// Interface for handling incoming messages.
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// The default delegate which applies to all message endpoints.
    /// If a delegate is already associated with the input, it
    /// will be called, else the default delegate will be called.
    /// </summary>
    /// <param name="message">The received message.</param>
    /// <param name="userContext">Context object passed in when the callback was registered.</param>
    /// <returns><inheritdoc cref="Task{MessageResponse}"/></returns>
    public Task<MessageResponse> Default(Message message, object userContext);
}
