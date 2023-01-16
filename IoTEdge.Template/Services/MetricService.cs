using IoTEdge.Template.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;

namespace IoTEdge.Template.Services;

/// <summary>
/// <see cref="IHostedService"/> for the <see cref="MetricServer"/>.
/// </summary>
public sealed class MetricService : IHostedService
{
	private readonly ILogger<MetricService> _logger;
	private readonly MetricOptions _options;
	private readonly IMetricServer _metricServer;

	/// <summary>
	/// Public <see cref="MetricService"/> constructor, parameters resolved through <b>Dependency injection</b>.
	/// </summary>
	/// <param name="logger"><see cref="ILogger"/> resolved through <b>Dependency injection</b>.</param>
	/// <param name="options"><see cref="IOptions{MetricOptions}"/> resolved through <b>Dependency injection</b>.</param>
	/// <exception cref="ArgumentNullException"></exception>
	public MetricService(ILogger<MetricService> logger, IOptions<MetricOptions> options)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_options = options.Value ?? throw new ArgumentNullException(nameof(options));

		_metricServer = new MetricServer(_options.HostName, _options.Port, _options.Url, useHttps: _options.UseHttps);
	}

	/// <inheritdoc cref="IHostedService.StartAsync(CancellationToken)"/>
	/// <remarks>Currently the <see cref="MetricServer"/> is allowed to fault.</remarks>
	public Task StartAsync(CancellationToken cancellationToken)
	{
		try
		{
			_metricServer.Start();
			_logger.LogInformation("Metrics server running at {MetricServer}", _options);
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "The MetricsServer has encountered an error. Disabling..");
		}

		return Task.CompletedTask;
	}

	/// <inheritdoc cref="IHostedService.StopAsync(CancellationToken)"/>
	public async Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Stopping Metrics server..");
		await _metricServer.StopAsync();
		_metricServer?.Dispose();
	}
}
