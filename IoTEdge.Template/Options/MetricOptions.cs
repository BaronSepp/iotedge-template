namespace IoTEdge.Template.Options;
public class MetricOptions
{
    public const string Section = "Metric";

    public string HostName { get; set; }

    public int Port { get; set; }

    public string Url { get; set; }

    public bool UseHttps { get; set; }

    public override string ToString()
    {
        return $"http{(UseHttps ? "s" : "")}://{HostName}:{Port}/{Url}";
    }
}
