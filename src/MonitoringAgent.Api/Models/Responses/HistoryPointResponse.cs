public sealed class HistoryPointResponse
{
    public DateTime TimestampUtc { get; set; }

    public decimal CpuPercent { get; set; }

    public decimal MemoryPercent { get; set; }

    public decimal DiskPercentUsed { get; set; }

    public decimal DiskReadLatencyMs { get; set; }

    public decimal DiskWriteLatencyMs { get; set; }

    public decimal AvgDiskQueueLength { get; set; }

    public decimal DiskReadsPerSec { get; set; }

    public decimal DiskWritesPerSec { get; set; }

    public decimal NetworkBytesReceivedPerSec { get; set; }

    public decimal NetworkBytesSentPerSec { get; set; }
}