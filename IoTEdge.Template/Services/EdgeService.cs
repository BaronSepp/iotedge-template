using IoTEdge.Template.IoTEdge;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTEdge.Template.Services;
public class EdgeService : BackgroundService
{
    private readonly IModuleClient _moduleClient;

    public EdgeService(IModuleClient moduleClient)
    {
        _moduleClient = moduleClient ?? throw new ArgumentNullException(nameof(moduleClient)); ;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _moduleClient.OpenAsync(stoppingToken).ConfigureAwait(false);
    }
}
