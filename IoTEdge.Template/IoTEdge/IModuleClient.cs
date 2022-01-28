using Microsoft.Azure.Devices.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTEdge.Template.Module.IoTEdge;

internal interface IModuleClient : IAsyncDisposable
{
    Task Init(CancellationToken stoppingToken);
    Task SendEventAsync(string output, Message message, CancellationToken stoppingToken = default);
}
