using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using InternalModuleClient = Microsoft.Azure.Devices.Client.ModuleClient;

namespace IoTEdge.Template.IoT;

/// <summary>
/// Provides a wrapper around <see cref="InternalModuleClient"/> for use with hosted services.
/// </summary>
public interface IModuleClient : IAsyncDisposable, IDisposable
{
	/// <summary>
	/// The ID of the module as defined in IoT Hub/Central.
	/// </summary>
	public string ModuleId { get; }

	/// <summary>
	/// The ID of the device as defined in IoT Hub/Central.
	/// </summary>
	public string DeviceId { get; }

	/// <inheritdoc cref="InternalModuleClient.OpenAsync(CancellationToken)"/>
	public Task OpenAsync(CancellationToken stoppingToken);

	/// <inheritdoc cref="InternalModuleClient.SendEventAsync(string, Message, CancellationToken)"/>
	public Task SendEventAsync(string output, Message message, CancellationToken stoppingToken = default);

	/// <inheritdoc cref="InternalModuleClient.SendEventBatchAsync(string, IEnumerable{Message}, CancellationToken)"/>
	public Task SendEventBatchAsync(string output, IEnumerable<Message> messages, CancellationToken stoppingToken = default);

	/// <inheritdoc cref="InternalModuleClient.UpdateReportedPropertiesAsync(TwinCollection, CancellationToken)"/>
	public Task UpdateReportedPropertiesAsync(TwinCollection desiredProperties, CancellationToken stoppingToken = default);

	/// <inheritdoc cref="InternalModuleClient.InvokeMethodAsync(string, string, MethodRequest, CancellationToken)"/>
	public Task<MethodResponse> InvokeMethodAsync(string deviceId, string moduleId, MethodRequest methodRequest, CancellationToken stoppingToken = default);
}
