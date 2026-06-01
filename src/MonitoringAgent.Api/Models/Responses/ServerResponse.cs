namespace MonitoringAgent.Api.Models.Responses;

public sealed class ServerResponse
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

    public decimal? CpuPercent
    { get; set; }

    public decimal? MemoryPercent
    { get; set; }

    public decimal? DiskPercentUsed
    { get; set; }

    public bool? GatewayReachable
    { get; set; }
}