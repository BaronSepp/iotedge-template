using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;

namespace IoTEdge.Template.IoT.TwinHandlers;

/// <summary>
/// Interface for handling module twin updates.
/// </summary>
public interface ITwinHandler
{
	/// <summary>
	///	Represents a read-only key/value pair collection based on the received <see cref="TwinCollection"/>
	/// </summary>
	public IReadOnlyDictionary<string, object> Twin { get; }

	/// <inheritdoc cref="DesiredPropertyUpdateCallback"/>
	public Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext);
}
