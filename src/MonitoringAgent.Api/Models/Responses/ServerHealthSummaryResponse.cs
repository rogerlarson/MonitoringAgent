namespace MonitoringAgent.Api.Models.Responses;

public sealed class ServerHealthSummaryResponse
{
    public int Healthy
    { get; set; }

    public int Warning
    { get; set; }

    public int Critical
    { get; set; }

    public int Offline
    { get; set; }
}