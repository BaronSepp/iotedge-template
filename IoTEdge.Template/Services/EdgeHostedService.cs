using IoTEdge.Template.IoTEdge;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTEdge.Template.Services;
public class EdgeHostedService : IHostedService
{
    private readonly IModuleClient _moduleClient;

    public EdgeHostedService(IModuleClient moduleClient)
    {
        _moduleClient = moduleClient ?? throw new ArgumentNullException(nameof(moduleClient)); ;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _moduleClient.Init(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _moduleClient.DisposeAsync();
    }
}
