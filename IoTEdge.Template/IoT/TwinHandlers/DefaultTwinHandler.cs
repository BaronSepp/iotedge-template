using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using Prometheus;
using System.Text.Json;

namespace IoTEdge.Template.IoT.TwinHandlers;

/// <summary>
/// Implementation for handling module twin updates.
/// </summary>
public sealed class DefaultTwinHandler : ITwinHandler
{
	private readonly Counter _twinUpdateCounter;
	private readonly ILogger<DefaultTwinHandler> _logger;
	private readonly IDictionary<string, JsonElement> _twin;

	/// <summary>
	/// EventHandler to subscribe on when the Desired Properties are changed.
	/// </summary>
	public event EventHandler TwinUpdated;

	/// <summary>
	/// Public <see cref="DefaultTwinHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
	public DefaultTwinHandler(ILogger<DefaultTwinHandler> logger)
	{
		_twinUpdateCounter = Metrics.CreateCounter("twin_updates_received", "Amount of twin updates received");
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_twin = new Dictionary<string, JsonElement>(0);
		TwinUpdated = (_, _) => { };
	}

	/// <summary>
	/// Get properties from Desired Properties in the Module Twin.
	/// </summary>
	/// <typeparam name="T">The type of the Desired Property.</typeparam>
	/// <param name="key">The keyname of the Desired Propeprty.</param>
	/// <returns>The desired property casted to the given Type.</returns>
	/// <exception cref="NullReferenceException"></exception>
	public T GetProperty<T>(string key)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			throw new ArgumentNullException(nameof(key));
		}

		if (_twin.TryGetValue(key, out var value) is false)
		{
			throw new ArgumentException("Property was not found!", key);
		}

		return value.Deserialize<T>() ?? throw new NullReferenceException($"Property {key} could not be parsed to type {typeof(T)}!");
	}


	/// <inheritdoc cref="ITwinHandler.OnDesiredPropertiesUpdate"/>
	public async Task OnDesiredPropertiesUpdate(TwinCollection desiredProperties, object userContext)
	{
		desiredProperties.ClearMetadata();
		_logger.LogInformation("Incoming desired properties: {properties}", desiredProperties.ToJson());

		if (userContext is IModuleClient moduleClient)
		{
			await moduleClient.UpdateReportedPropertiesAsync(desiredProperties);
		}

		// Get the ModuleTwin's Desired Properties
		GetDesiredProperties(desiredProperties);

		// Invoke event
		TwinUpdated.Invoke(this, EventArgs.Empty);

		_twinUpdateCounter.Inc();
	}


	/// <summary>
	/// Get the Desired Properties.
	/// Adds them as dictionary pair for later retrieval.
	/// </summary>
	/// <param name="twinCollection"></param>
	/// <exception cref="NullReferenceException"></exception>
	private void GetDesiredProperties(TwinCollection twinCollection)
	{
		var jsonString = twinCollection.ToJson();

		if (string.IsNullOrWhiteSpace(jsonString))
		{
			throw new NullReferenceException(nameof(jsonString));
		}

		using var jsonDocument = JsonDocument.Parse(jsonString);
		var documentRoot = jsonDocument.RootElement;

		_twin.Clear();
		foreach (var item in documentRoot.EnumerateObject())
		{
			_twin.Add(item.Name, item.Value.Clone());
		}

		_logger.LogDebug("Twin dictionary set: {dict}", JsonSerializer.Serialize(_twin));
	}
}
