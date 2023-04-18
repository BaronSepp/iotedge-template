using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Text.Json;

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
	public Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext);

	/// <summary>
	/// Get properties from Desired Properties in the Module Twin.
	/// </summary>
	/// <param name="key">The keyname of the Desired Propeprty.</param>
	/// <returns>The desired property as <see cref="JsonElement"/>.</returns>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentException"></exception>
	public JsonElement GetProperty(string key);

	/// <returns>The desired property casted to the given Type.</returns>
	/// <exception cref="NullReferenceException"></exception>
	/// <inheritdoc cref="GetProperty"/>
	public T GetProperty<T>(string key);

	/// <summary>
	/// Event to subscribe on when Module Twin is updated.
	/// </summary>
	event EventHandler TwinUpdated;
}
