// ============================================================================
// Project: MonitoringAgent.Api
// File: HistoryPointResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a historical host performance data point.
//
//      Used by dashboard charts and trend views to display resource
//      utilization, disk performance, and network activity over time.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a historical host metrics snapshot.
/// </summary>
public sealed class HistoryPointResponse
{
    // =====================================================================
    // Snapshot Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp associated with the data point.
    /// </summary>
    public DateTime TimestampUtc
    {
        get;
        set;
    }

    // =====================================================================
    // System Metrics
    // =====================================================================

    /// <summary>
    /// CPU utilization percentage.
    /// </summary>
    public decimal CpuPercent
    {
        get;
        set;
    }

    /// <summary>
    /// Memory utilization percentage.
    /// </summary>
    public decimal MemoryPercent
    {
        get;
        set;
    }

    // =====================================================================
    // Disk Metrics
    // =====================================================================

    /// <summary>
    /// Percentage of disk space currently in use.
    /// </summary>
    public decimal DiskPercentUsed
    {
        get;
        set;
    }

    /// <summary>
    /// Average disk read latency in milliseconds.
    /// </summary>
    public decimal DiskReadLatencyMs
    {
        get;
        set;
    }

    /// <summary>
    /// Average disk write latency in milliseconds.
    /// </summary>
    public decimal DiskWriteLatencyMs
    {
        get;
        set;
    }

    /// <summary>
    /// Average disk queue length.
    /// </summary>
    public decimal AvgDiskQueueLength
    {
        get;
        set;
    }

    /// <summary>
    /// Disk read operations per second.
    /// </summary>
    public decimal DiskReadsPerSec
    {
        get;
        set;
    }

    /// <summary>
    /// Disk write operations per second.
    /// </summary>
    public decimal DiskWritesPerSec
    {
        get;
        set;
    }

    // =====================================================================
    // Network Metrics
    // =====================================================================

    /// <summary>
    /// Network bytes received per second.
    /// </summary>
    public decimal NetworkBytesReceivedPerSec
    {
        get;
        set;
    }

    /// <summary>
    /// Network bytes sent per second.
    /// </summary>
    public decimal NetworkBytesSentPerSec
    {
        get;
        set;
    }
}