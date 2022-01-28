using IoTEdge.Template.Module.IoTEdge;
using IoTEdge.Template.Module.IoTEdge.Handlers;
using IoTEdge.Template.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IoTEdge.Template.Module;
public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        await host.RunAsync();

        return Environment.ExitCode;
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(c =>
                {
                    c.UseUtcTimestamp = true;
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.mmm] ";
                });
            })
            .ConfigureServices((_, services) =>
            {
                services.AddSingleton<IMessageHandler, MessageHandler>();
                services.AddSingleton<IMethodHandler, MethodHandler>();
                services.AddSingleton<ITwinHandler, TwinHandler>();
                services.AddSingleton<IMessageHandler, MessageHandler>();
                services.AddSingleton<IModuleClient, ModuleClient>();
                services.AddHostedService<EdgeHostedService>();

            })
            .UseConsoleLifetime();
    }
}

