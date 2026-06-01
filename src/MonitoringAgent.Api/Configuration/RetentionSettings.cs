namespace MonitoringAgent.Api.Configuration;

public sealed class RetentionSettings
{
    public int HostSnapshotDays
    { get; set; } = 30;

    public int GatewaySnapshotDays
    { get; set; } = 30;

    public int IgnitionSnapshotDays
    { get; set; } = 30;

    public int AlertEventDays
    { get; set; } = 365;
}