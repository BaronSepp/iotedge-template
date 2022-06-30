using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace IoTEdge.Template.IoT.MessageHandlers;

/// <summary>
/// The default delegate which applies to all message endpoints.
/// If a delegate is already associated with the input, it
/// will be called, else the default delegate will be called.
/// </summary>
public sealed class DefaultMessageHandler : IMessageHandler
{
	private readonly Counter _unhandledMessageCounter;
	private readonly ILogger<DefaultMessageHandler> _logger;

	/// <summary>
	/// Public <see cref="DefaultMessageHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
	public DefaultMessageHandler(ILogger<DefaultMessageHandler> logger)
	{
		_unhandledMessageCounter = Metrics.CreateCounter("unhandled_message_counter", "Amount of unhandled messages received");
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	/// <inheritdoc cref="IMessageHandler.InputName"/>
	public string InputName => "*";

	/// <inheritdoc cref="IMessageHandler.Handle"/>
	public Task<MessageResponse> Handle(Message message, object userContext)
	{
		_unhandledMessageCounter.Inc();
		_logger.LogInformation("Unhandled message received from module {module}", message.ConnectionModuleId);
		return Task.FromResult(MessageResponse.Completed);
	}
}
