namespace MonitoringAgent.Api.Models.Responses;

public sealed class DashboardSummaryResponse
{
    public int ServerCount
    { get; set; }

    public int OnlineServers
    { get; set; }

    public int OfflineServers
    { get; set; }

    public int OpenAlerts
    { get; set; }

    public int CriticalAlerts
    { get; set; }

    public int WarningAlerts
    { get; set; }
}