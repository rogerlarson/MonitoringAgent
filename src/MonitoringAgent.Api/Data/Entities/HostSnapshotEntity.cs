namespace MonitoringAgent.Api.Data.Entities;

public sealed class HostSnapshotEntity
{
    public long SnapshotId { get; set; }

    public int ServerId { get; set; }

    public DateTime SnapshotUtc { get; set; }

    // System

    public decimal CpuPercent { get; set; }

    public decimal MemoryPercent { get; set; }

    public long AvailableMemoryMb { get; set; }

    public int ProcessCount { get; set; }

    public long SystemUptimeMinutes { get; set; }

    public decimal ContextSwitchesPerSec { get; set; }

    public decimal PageFaultsPerSec { get; set; }

    // Disk

    public string? SystemDrive { get; set; }

    public decimal DiskPercentUsed { get; set; }

    public decimal DiskFreeGb { get; set; }

    public decimal DiskReadsPerSec { get; set; }

    public decimal DiskWritesPerSec { get; set; }

    public decimal DiskReadLatencyMs { get; set; }

    public decimal DiskWriteLatencyMs { get; set; }

    public decimal DiskQueueLength { get; set; }

    public decimal AvgDiskQueueLength { get; set; }

    // Network

    public string? PrimaryNetworkInterface { get; set; }

    public decimal NetworkBytesReceivedPerSec { get; set; }

    public decimal NetworkBytesSentPerSec { get; set; }

    public long NetworkReceiveErrors { get; set; }

    public long NetworkSendErrors { get; set; }

    public long NetworkReceiveDiscards { get; set; }

    public long NetworkSendDiscards { get; set; }

    public decimal TcpRetransmissionsPerSec { get; set; }

    // Metadata

    public DateTime CreatedDateUtc { get; set; }

    public ServerEntity Server { get; set; }
        = null!;
}