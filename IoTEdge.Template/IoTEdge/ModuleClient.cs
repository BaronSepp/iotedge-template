using IoTEdge.Template.IoTEdge.Handlers;
using IoTEdge.Template.Options;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using InternalModuleClient = Microsoft.Azure.Devices.Client.ModuleClient;

namespace IoTEdge.Template.IoTEdge;

/// <summary>
/// An implemented wrapper around <see cref="InternalModuleClient"/> for use with hosted services.
/// </summary>
public sealed class ModuleClient : IModuleClient
{
    private readonly ILogger<ModuleClient> _logger;
    private readonly IMessageHandler _messageHandler;
    private readonly ITwinHandler _twinHandler;
    private readonly IMethodHandler _methodHandler;
    private readonly IConnectionHandler _connectionHandler;
    private readonly ModuleClientOptions _moduleClientOptions;

    private InternalModuleClient _moduleClient;

    /// <summary>
    /// Public <see cref="ModuleClient"/> constructor, parameters resolved through <b>Dependency injection</b>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
    /// <param name="messageHandler"><see cref="IMessageHandler"/> resolved through <b>Dependency injection</b>.</param>
    /// <param name="twinHandler"><see cref="ITwinHandler"/> resolved through <b>Dependency injection</b>.</param>
    /// <param name="methodHandler"><see cref="IMethodHandler"/> resolved through <b>Dependency injection</b>.</param>
    /// <param name="connectionHandler"><see cref="IConnectionHandler"/> resolved through <b>Dependency injection</b>.</param>
    /// <param name="moduleClientOptions"><see cref="IOptions{ModuleClientOptions}"/> resolved through <b>Dependency injection</b>.</param>
    /// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
    public ModuleClient(
        ILogger<ModuleClient> logger,
        IMessageHandler messageHandler,
        ITwinHandler twinHandler,
        IMethodHandler methodHandler,
        IConnectionHandler connectionHandler,
        IOptions<ModuleClientOptions> moduleClientOptions)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
        _twinHandler = twinHandler ?? throw new ArgumentNullException(nameof(twinHandler));
        _methodHandler = methodHandler ?? throw new ArgumentNullException(nameof(methodHandler));
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
        await _moduleClient.SetDesiredPropertyUpdateCallbackAsync(_twinHandler.OnDesiredPropertiesUpdate, null, stoppingToken).ConfigureAwait(false);
        _logger.LogDebug("Twin handler ready.");

        // Method Handlers
        await _moduleClient.SetMethodDefaultHandlerAsync(_methodHandler.Default, null, stoppingToken).ConfigureAwait(false);
        _logger.LogDebug("Method handlers ready.");

        // Message Handlers
        await _moduleClient.SetMessageHandlerAsync(_messageHandler.Default, null, stoppingToken).ConfigureAwait(false);
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

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("Disposing ModuleClient asynchronousy..");
        if (_moduleClient is IAsyncDisposable disposable) await disposable.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _logger.LogDebug("Disposing ModuleClient..");
        _moduleClient?.CloseAsync().Wait();
        _moduleClient?.Dispose();
    }
}
