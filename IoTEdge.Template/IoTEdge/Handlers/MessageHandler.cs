using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.Module.IoTEdge.Handlers;
public sealed class MessageHandler : IMessageHandler
{
    private readonly ILogger<MessageHandler> _logger;

    public MessageHandler(ILogger<MessageHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<MessageResponse> Default(Message message, object userContext)
    {
        _logger.LogInformation("Unhandled message received from module {module}", message.ConnectionModuleId);
        return Task.FromResult(MessageResponse.Completed);
    }
}
