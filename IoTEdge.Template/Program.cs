using IoTEdge.Template.IoTEdge;
using IoTEdge.Template.IoTEdge.Handlers;
using IoTEdge.Template.Options;
using IoTEdge.Template.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTEdge.Template;
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
        return Host.CreateDefaultBuilder()
#if DEBUG
            .UseEnvironment("Development")
#endif
            .ConfigureAppConfiguration(app =>
            {
                var switchMappings = new Dictionary<string, string>
                {
                    {"-u", "ModuleClient:UpstreamProtocol"},
                    {"--UpstreamProtocol", "ModuleClient:UpstreamProtocol"},
                    {"-v", "Logging:LogLevel:Default"},
                    {"--Verbosity", "Logging:LogLevel:Default"},
                };
                app.AddCommandLine(args, switchMappings);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(c =>
                {
                    c.UseUtcTimestamp = true;
                    c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.mmm] ";
                });
            })
            .ConfigureServices((host, services) =>
            {

                services.AddHostedService<EdgeService>();
                services.AddHostedService<MetricService>();

                services.AddOptions();
                services.Configure<ModuleClientOptions>(host.Configuration.GetRequiredSection(ModuleClientOptions.Section));
                services.Configure<MetricOptions>(host.Configuration.GetRequiredSection(MetricOptions.Section));

                services.AddSingleton<IMessageHandler, MessageHandler>();
                services.AddSingleton<IMethodHandler, MethodHandler>();
                services.AddSingleton<ITwinHandler, TwinHandler>();
                services.AddSingleton<IMessageHandler, MessageHandler>();
                services.AddSingleton<IConnectionHandler, ConnectionHandler>();
                services.AddSingleton<IModuleClient, ModuleClient>();
            })
            .UseConsoleLifetime();
    }
}

