namespace MonitoringAgent.Api.Models.Responses;

public sealed class HostSnapshotHistoryResponse
{
    public DateTime SnapshotUtc
    { get; set; }

    public decimal CpuPercent
    { get; set; }

    public decimal MemoryPercent
    { get; set; }

    public decimal DiskPercentUsed
    { get; set; }
}