public sealed class HostMetricsResponse
{
    public decimal CpuPercent
    { get; set; }

    public decimal MemoryPercent
    { get; set; }

    public decimal DiskPercentUsed
    { get; set; }

    public long AvailableMemoryMb
    { get; set; }

    public long SystemUptimeMinutes
    { get; set; }
}