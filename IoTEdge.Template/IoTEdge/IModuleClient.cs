using Microsoft.Azure.Devices.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoTEdge.Template.IoTEdge;
public interface IModuleClient : IAsyncDisposable, IDisposable
{
    Task OpenAsync(CancellationToken stoppingToken);
    Task SendEventAsync(string output, Message message, CancellationToken stoppingToken = default);
}
