using IoTEdge.Template.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prometheus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTEdge.Template.Services;
public sealed class MetricService : IHostedService
{
    private readonly ILogger<MetricService> _logger;
    private readonly MetricOptions _options;
    private readonly IMetricServer _metricServer;

    public MetricService(ILogger<MetricService> logger, IOptions<MetricOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));

        _metricServer = new MetricServer(_options.HostName, _options.Port, _options.Url, useHttps: _options.UseHttps);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _metricServer.Start();
            _logger.LogInformation("Metrics server running at {MetricServer}", _options);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to start Metrics server.");
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Metrics server..");
        await _metricServer?.StopAsync();
        _metricServer?.Dispose();
    }
}
