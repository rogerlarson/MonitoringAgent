namespace MonitoringAgent.Api.Models.Responses;

public sealed class IgnitionStatusResponse
{
    public int ServerServiceId
    { get; set; }

    public string ServiceName
    { get; set; }
        = string.Empty;

    public DateTime SnapshotUtc
    { get; set; }

    public string Status
    { get; set; }
        = "Unknown";

    public bool ServiceRunning
    { get; set; }

    public bool ProcessRunning
    { get; set; }

    public decimal CpuPercent
    { get; set; }

    public long MemoryMb
    { get; set; }

    public int ThreadCount
    { get; set; }

    public int HandleCount
    { get; set; }

    public long UptimeMinutes
    { get; set; }

    public int ProcessId
    { get; set; }

    public string? ProcessName
    { get; set; }

    public string? IgnitionVersion
    { get; set; }

    public string? JavaVersion
    { get; set; }
}