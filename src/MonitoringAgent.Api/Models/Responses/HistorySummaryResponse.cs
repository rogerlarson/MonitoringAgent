namespace MonitoringAgent.Api.Models.Responses;

public sealed class HistorySummaryResponse
{
    public decimal CpuAverage { get; set; }

    public decimal CpuMaximum { get; set; }

    public decimal MemoryAverage { get; set; }

    public decimal MemoryMaximum { get; set; }

    public decimal DiskAverage { get; set; }

    public decimal DiskMaximum { get; set; }

    public decimal DiskReadLatencyMaximum { get; set; }

    public decimal DiskWriteLatencyMaximum { get; set; }

    public decimal AvgDiskQueueMaximum { get; set; }

    public decimal NetworkInMaximum { get; set; }

    public decimal NetworkOutMaximum { get; set; }
}