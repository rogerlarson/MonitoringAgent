public sealed class DashboardTrendResponse
{
    public decimal CpuAverage
    { get; set; }

    public decimal CpuMaximum
    { get; set; }

    public decimal MemoryAverage
    { get; set; }

    public decimal MemoryMaximum
    { get; set; }

    public decimal DiskAverage
    { get; set; }

    public decimal DiskMaximum
    { get; set; }

    public decimal GatewayResponseAverageMs
    { get; set; }

    public int TotalAlertsOpened
    { get; set; }

    public int CriticalAlertsOpened
    { get; set; }

    public int WarningAlertsOpened
    { get; set; }

    public int HealthyServers
    { get; set; }

    public int WarningServers
    { get; set; }

    public int CriticalServers
    { get; set; }

    public int OfflineServers
    { get; set; }
}