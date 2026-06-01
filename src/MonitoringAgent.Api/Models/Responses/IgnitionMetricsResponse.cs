public sealed class IgnitionMetricsResponse
{
    public bool ProcessRunning
    { get; set; }

    public bool ServiceRunning
    { get; set; }

    public string? IgnitionVersion
    { get; set; }

    public string? JavaVersion
    { get; set; }

    public decimal CpuPercent
    { get; set; }

    public long MemoryMb
    { get; set; }

    public long UptimeMinutes
    { get; set; }

    public int GatewayResponseMs 
    { get; set; }

    public long IgnitionMemoryMb 
    { get; set; }

    public decimal IgnitionCpuPercent 
    { get; set; }
}