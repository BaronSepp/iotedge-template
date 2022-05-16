using IoTEdge.Template.IoT;
using IoTEdge.Template.IoT.ConnectionHandlers;
using IoTEdge.Template.IoT.MessageHandlers;
using IoTEdge.Template.IoT.MethodHandlers;
using IoTEdge.Template.IoT.TwinHandlers;
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

/// <summary>
/// The starting point of the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// Initialization of the application.
    /// </summary>
    /// <param name="args">Command line arguments in key/value pair format.</param>
    /// <returns><inheritdoc cref="Environment.ExitCode"/></returns>
    public static async Task<int> Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        await host.RunAsync();

        return Environment.ExitCode;
    }

    /// <summary>
    /// Initializes and configures a new instance of the <see cref="HostBuilder"/> class.
    /// </summary>
    /// <param name="args">Command line arguments in key/value pair format.</param>
    /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                IDictionary<string, string> switchMappings = new Dictionary<string, string>
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
                services.AddOptions();
                services.Configure<ModuleClientOptions>(host.Configuration.GetRequiredSection(ModuleClientOptions.Section));
                services.Configure<MetricOptions>(host.Configuration.GetRequiredSection(MetricOptions.Section));

                services.AddSingleton<IMessageHandler, DefaultMessageHandler>();
                services.AddSingleton<IMethodHandler, DefaultMethodHandler>();
                services.AddSingleton<ITwinHandler, TwinHandler>();
                services.AddSingleton<IConnectionHandler, ConnectionHandler>();
                services.AddSingleton<IModuleClient, ModuleClient>();

                services.AddHostedService<EdgeService>();
                services.AddHostedService<MetricService>();
            })
            .UseConsoleLifetime();
    }
}
