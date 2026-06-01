namespace MonitoringAgent.Api.Configuration;

public sealed class LogSettings
{
    public string LogDirectory
    { get; set; } = "Logs";
    public int RetentionDays
    { get; set; } = 30;

    public bool EnableApiLogging
    { get; set; } = true;

    public bool EnableHeartbeatLogging
    { get; set; } = true;

    public bool EnableAlertLogging
    { get; set; } = true;

    public bool EnableEmailLogging
    { get; set; } = true;

    public bool EnableMaintenanceLogging
    { get; set; } = true;
}