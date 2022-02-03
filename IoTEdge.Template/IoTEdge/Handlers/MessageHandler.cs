using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge.Handlers;
public sealed class MessageHandler : IMessageHandler
{
    private readonly ILogger<MessageHandler> _logger;

    // Metrics
    private readonly Counter UnhandledMessageCounter =
        Metrics.CreateCounter("unhandled_message_counter", "Amount of unhandled messages received");

    public MessageHandler(ILogger<MessageHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<MessageResponse> Default(Message message, object userContext)
    {
        UnhandledMessageCounter.Inc();
        _logger.LogInformation("Unhandled message received from module {module}", message.ConnectionModuleId);
        return Task.FromResult(MessageResponse.Completed);
    }
}
