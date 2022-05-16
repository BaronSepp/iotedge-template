using Microsoft.Azure.Devices.Client;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoT.MessageHandlers;

/// <summary>
/// Interface for handling incoming messages.
/// </summary>
public interface IMessageHandler
{
	/// <summary>
	/// The name of the input to associate with the delegate.
	/// </summary>
	public string InputName { get; }

	/// <summary>
	/// The delegate to be used when a message is sent to the particular InputName.
	/// </summary>
	/// <param name="message">The received message.</param>
	/// <param name="userContext">Context object passed in when the callback was registered.</param>
	/// <returns>The MessageResponse.</returns>
	public Task<MessageResponse> Handle(Message message, object userContext);
}
