using MonitoringAgent.Api.Models.Responses;

public sealed class ServerDetailsResponse
{
    public int ServerId
    { get; set; }

    public string ServerName
    { get; set; }
        = string.Empty;

    public DateTime? LastSeenUtc
    { get; set; }

    public string Status
    { get; set; }
        = "Unknown";

    public int ServiceCount
    { get; set; }

    public string DomainName
    { get; set; }
    = string.Empty;

    public string AgentVersion
    { get; set; }
        = string.Empty;

    public string OperatingSystem
    { get; set; }
        = string.Empty;

    public string OperatingSystemVersion
    { get; set; }
        = string.Empty;

    public int? ProcessorCount
    { get; set; }

    public long? TotalMemoryMb
    { get; set; }

    public HostMetricsResponse Host
    { get; set; } = new();

    public GatewayMetricsResponse? Gateway
    { get; set; }

    public IgnitionMetricsResponse? Ignition
    { get; set; }

    public List<RecentAlertResponse> OpenAlerts
    { get; set; } = new();
}