namespace MonitoringAgent.Api.Models.Responses;

public sealed class IgnitionHistoryResponse
{
    public DateTime SnapshotUtc
    { get; set; }

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
}