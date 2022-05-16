using IoTEdge.Template.IoT.ConnectionHandlers;
using IoTEdge.Template.IoT.MessageHandlers;
using IoTEdge.Template.IoT.MethodHandlers;
using IoTEdge.Template.IoT.TwinHandlers;
using IoTEdge.Template.Options;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InternalModuleClient = Microsoft.Azure.Devices.Client.ModuleClient;

namespace IoTEdge.Template.IoT;

/// <summary>
/// An implemented wrapper around <see cref="InternalModuleClient"/> for use with hosted services.
/// </summary>
public sealed class ModuleClient : IModuleClient
{
	private readonly ILogger<ModuleClient> _logger;
	private readonly IEnumerable<IMessageHandler> _messageHandlers;
	private readonly ITwinHandler _twinHandler;
	private readonly IEnumerable<IMethodHandler> _methodHandlers;
	private readonly IConnectionHandler _connectionHandler;
	private readonly ModuleClientOptions _moduleClientOptions;

	private InternalModuleClient _moduleClient;

	/// <summary>
	/// Public <see cref="ModuleClient"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="messageHandlers"><see cref="IMethodHandler"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="twinHandler"><see cref="ITwinHandler"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="methodHandlers"><see cref="IMethodHandler"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="connectionHandler"><see cref="IConnectionHandler"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="moduleClientOptions"><see cref="IOptions{ModuleClientOptions}"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
	public ModuleClient(
		ILogger<ModuleClient> logger,
		IEnumerable<IMessageHandler> messageHandlers,
		ITwinHandler twinHandler,
		IEnumerable<IMethodHandler> methodHandlers,
		IConnectionHandler connectionHandler,
		IOptions<ModuleClientOptions> moduleClientOptions)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_messageHandlers = messageHandlers ?? throw new ArgumentNullException(nameof(messageHandlers));
		_twinHandler = twinHandler ?? throw new ArgumentNullException(nameof(twinHandler));
		_methodHandlers = methodHandlers ?? throw new ArgumentNullException(nameof(methodHandlers));
		_connectionHandler = connectionHandler ?? throw new ArgumentNullException(nameof(connectionHandler));
		_moduleClientOptions = moduleClientOptions.Value ?? throw new ArgumentNullException(nameof(moduleClientOptions));
	}

	/// <inheritdoc cref="IModuleClient.OpenAsync(CancellationToken)" />
	public async Task OpenAsync(CancellationToken stoppingToken)
	{
		// Initialize the Edge runtime
		var upstreamProtocol = _moduleClientOptions.GetUpstreamProtocol();
		_moduleClient = await InternalModuleClient.CreateFromEnvironmentAsync(upstreamProtocol).ConfigureAwait(false);
		_logger.LogDebug("Initialized ModuleClient using {UpstreamProtocol}.", upstreamProtocol);

		// Connection Handler
		_moduleClient.SetConnectionStatusChangesHandler(_connectionHandler.OnConnectionChange);
		_logger.LogDebug("Connection handler ready.");

		// Twin Handler
		var twin = await _moduleClient.GetTwinAsync(stoppingToken);
		await _twinHandler.OnDesiredPropertiesUpdate(twin.Properties.Desired, this);
		await _moduleClient.SetDesiredPropertyUpdateCallbackAsync(_twinHandler.OnDesiredPropertiesUpdate, this, stoppingToken).ConfigureAwait(false);
		_logger.LogDebug("Twin handler ready.");

		// Method Handlers
		foreach (var methodHandler in _methodHandlers)
		{
			if (methodHandler.MethodName == "*")
			{
				await _moduleClient.SetMethodDefaultHandlerAsync(methodHandler.Handle, this, stoppingToken).ConfigureAwait(false);
				continue;
			}

			await _moduleClient.SetMethodHandlerAsync(methodHandler.MethodName, methodHandler.Handle, this, stoppingToken).ConfigureAwait(false);
		}
		_logger.LogDebug("Method handlers ready.");

		// Message Handlers
		foreach (var messageHandler in _messageHandlers)
		{
			if (messageHandler.InputName == "*")
			{
				await _moduleClient.SetMessageHandlerAsync(messageHandler.Handle, this, stoppingToken).ConfigureAwait(false);
				continue;
			}

			await _moduleClient.SetInputMessageHandlerAsync(messageHandler.InputName, messageHandler.Handle, this, stoppingToken).ConfigureAwait(false);
		}
		_logger.LogDebug("Message handlers ready.");

		// Open the ModuleClient instance
		await _moduleClient.OpenAsync(stoppingToken).ConfigureAwait(false);
		_logger.LogDebug("ModuleClient ready.");
	}

	/// <inheritdoc cref="IModuleClient.SendEventAsync"/>
	public async Task SendEventAsync(string output, Message message, CancellationToken stoppingToken = default)
	{
		await _moduleClient.SendEventAsync(output, message, stoppingToken);
	}

	/// <inheritdoc cref="IModuleClient.SendEventBatchAsync"/>
	public async Task SendEventBatchAsync(string output, IEnumerable<Message> messages, CancellationToken stoppingToken = default)
	{
		await _moduleClient.SendEventBatchAsync(output, messages, stoppingToken);
	}

	/// <inheritdoc cref="IModuleClient.UpdateReportedPropertiesAsync"/>
	public async Task UpdateReportedPropertiesAsync(TwinCollection desiredProperties, CancellationToken stoppingToken = default)
	{
		await _moduleClient.UpdateReportedPropertiesAsync(desiredProperties, stoppingToken);
	}

	/// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
	public async ValueTask DisposeAsync()
	{
		_logger.LogDebug("Disposing ModuleClient asynchronously..");
		if (_moduleClient is IAsyncDisposable disposable)
		{
			await disposable.DisposeAsync().ConfigureAwait(false);
		}
	}

	/// <inheritdoc cref="IDisposable.Dispose"/>
	public void Dispose()
	{
		_logger.LogDebug("Disposing ModuleClient..");
		_moduleClient?.CloseAsync().Wait();
		_moduleClient?.Dispose();
	}
}
