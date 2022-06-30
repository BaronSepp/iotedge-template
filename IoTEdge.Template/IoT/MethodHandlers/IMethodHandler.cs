using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.IoT.MethodHandlers;

/// <summary>
/// Interface for handling C2D method calls.
/// </summary>
public interface IMethodHandler
{
	/// <summary>
	/// The name of the method to associate with the delegate.
	/// </summary>
	public string MethodName { get; }

	/// <summary>
	/// The delegate to be used when a method with the given name is called by the cloud service.
	/// </summary>
	/// <param name="method">Class with details about method.</param>
	/// <param name="userContext">Context object passed in when the callback was registered.</param>
	/// <returns>The MethodResponse.</returns>
	public Task<MethodResponse> Handle(MethodRequest method, object userContext);
}
