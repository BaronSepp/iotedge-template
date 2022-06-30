using IoTEdge.Template.IoT;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IoTEdge.Template.Services;

/// <summary>
/// <see cref="BackgroundService"/> for the <see cref="ModuleClient"/>.
/// </summary>
public sealed class EdgeService : BackgroundService
{
	private readonly ILogger<EdgeService> _logger;
	private readonly IModuleClient _moduleClient;

	/// <summary>
	/// Public <see cref="EdgeService"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="moduleClient"><see cref="IModuleClient"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException">Thrown when any of the parameters could not be resolved.</exception>
	public EdgeService(ILogger<EdgeService> logger, IModuleClient moduleClient)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_moduleClient = moduleClient ?? throw new ArgumentNullException(nameof(moduleClient));
	}

	/// <inheritdoc cref="BackgroundService.ExecuteAsync(CancellationToken)"/>
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		try
		{
			await _moduleClient.OpenAsync(stoppingToken).ConfigureAwait(false);
			_logger.LogInformation("Successfully started the ModuleClient.");
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "The ModuleClient encountered a critical error.");
			throw;
		}
	}
}
