using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;

/// <summary>
/// Implementation for handling incoming messages.
/// </summary>
public sealed class MessageHandler : IMessageHandler
{
    private readonly Counter _unhandledMessageCounter;
    private readonly ILogger<MessageHandler> _logger;

    /// <summary>
    /// Public <see cref="MessageHandler"/> constructor, parameters resolved through <b>Dependency injection</b>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
    public MessageHandler(ILogger<MessageHandler> logger)
    {
        _unhandledMessageCounter = Metrics.CreateCounter("unhandled_message_counter", "Amount of unhandled messages received");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc cref="IMessageHandler.Default(Message, object)"/>
    public Task<MessageResponse> Default(Message message, object userContext)
    {
        _unhandledMessageCounter.Inc();
        _logger.LogInformation("Unhandled message received from module {module}", message.ConnectionModuleId);
        return Task.FromResult(MessageResponse.Completed);
    }
}
