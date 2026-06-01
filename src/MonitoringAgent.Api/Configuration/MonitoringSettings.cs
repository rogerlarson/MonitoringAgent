namespace MonitoringAgent.Api.Configuration;

public sealed class MonitoringSettings
{
    public int OfflineThresholdMinutes
    { get; set; } = 2;

    public int HeartbeatCheckIntervalSeconds
    { get; set; } = 60;

    public bool RequireApiKey
    { get; set; }

    public string ApiKey
    { get; set; } = string.Empty;
}