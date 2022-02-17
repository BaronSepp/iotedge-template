using Microsoft.Azure.Devices.Client;

namespace IoTEdge.Template.Options;

/// <summary>
/// Options to configure the IoTEdge Service.
/// These are overridden by the Module Twin if set.
/// </summary>
public class ModuleClientOptions
{
    /// <summary>
    /// The section where these settings are located in the appsettings.json
    /// </summary>
    public const string Section = "ModuleClient";

    /// <summary>The protocol to use for communication with IoT Hub.</summary>
    public string UpstreamProtocol { get; set; }

    /// <summary>Get the UpstreamProtocol from current options.</summary>
    /// <returns>The <see cref="TransportType">TransportType</see> to use.</returns>
    public TransportType GetUpstreamProtocol()
    {
        return UpstreamProtocol.ToLower().Trim() switch
        {
            "amqp" => TransportType.Amqp,
            "amqpws" => TransportType.Amqp_WebSocket_Only,
            "amqptcp" => TransportType.Amqp_Tcp_Only,
            "mqtt" => TransportType.Mqtt,
            "mqttws" => TransportType.Mqtt_WebSocket_Only,
            "mqtttcp" => TransportType.Mqtt_Tcp_Only,
            "http" => TransportType.Http1,
            _ => TransportType.Amqp_Tcp_Only
        };
    }
}
