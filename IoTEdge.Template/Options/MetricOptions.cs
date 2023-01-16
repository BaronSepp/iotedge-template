using Prometheus;

namespace IoTEdge.Template.Options;

/// <summary>
/// Options to configure the <see cref="MetricServer"/>.
/// </summary>
public sealed class MetricOptions
{
	/// <summary>
	/// The section where these settings are located in the appsettings.json
	/// </summary>
	public const string Section = "Metric";

	/// <summary> The Hostname of the <see cref="MetricServer"/>.</summary>
	public string HostName { get; set; } = "localhost";

	/// <summary> The Port number of the <see cref="MetricServer"/></summary>
	public int Port { get; set; } = 9600;

	/// <summary> The URL where the <see cref="MetricServer"/> will be accessible.</summary>
	public string Url { get; set; } = "metrics/";

	/// <summary> Whether the <see cref="MetricServer"/> uses HTTP over TLS.</summary>
	/// <remarks> Recommended in production environments.</remarks>
	public bool UseHttps { get; set; }

	/// <summary><inheritdoc cref="object.ToString"/></summary>
	/// <returns>The full address of the <see cref="MetricServer"/>.</returns>
	public override string ToString()
	{
		return $"http{(UseHttps ? "s" : "")}://{HostName}:{Port}/{Url}";
	}
}
