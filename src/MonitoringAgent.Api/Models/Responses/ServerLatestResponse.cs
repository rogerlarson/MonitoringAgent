// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerLatestResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the most recent host snapshot collected for a server.
//
//      Includes current CPU, memory, disk, and network performance metrics
//      used by server detail pages, diagnostics, and monitoring dashboards.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents the latest host metrics snapshot.
/// </summary>
public sealed class ServerLatestResponse
{
    // =====================================================================
    // Snapshot Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the snapshot was collected.
    /// </summary>
    public DateTime SnapshotUtc
    {
        get;
        set;
    }

    // =====================================================================
    // CPU & Memory Metrics
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

    /// <summary>
    /// Available physical memory in megabytes.
    /// </summary>
    public long AvailableMemoryMb
    {
        get;
        set;
    }

    /// <summary>
    /// Number of running processes.
    /// </summary>
    public int ProcessCount
    {
        get;
        set;
    }

    /// <summary>
    /// System uptime in minutes.
    /// </summary>
    public long SystemUptimeMinutes
    {
        get;
        set;
    }

    /// <summary>
    /// Context switches per second.
    /// </summary>
    public decimal ContextSwitchesPerSec
    {
        get;
        set;
    }

    /// <summary>
    /// Page faults per second.
    /// </summary>
    public decimal PageFaultsPerSec
    {
        get;
        set;
    }

    // =====================================================================
    // Disk Capacity Metrics
    // =====================================================================

    /// <summary>
    /// Primary monitored drive.
    /// </summary>
    public string? SystemDrive
    {
        get;
        set;
    }

    /// <summary>
    /// Percentage of disk space currently in use.
    /// </summary>
    public decimal DiskPercentUsed
    {
        get;
        set;
    }

    /// <summary>
    /// Available disk space in gigabytes.
    /// </summary>
    public decimal DiskFreeGb
    {
        get;
        set;
    }

    // =====================================================================
    // Disk Performance Metrics
    // =====================================================================

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
    /// Current disk queue length.
    /// </summary>
    public decimal DiskQueueLength
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

    // =====================================================================
    // Network Metrics
    // =====================================================================

    /// <summary>
    /// Primary network interface.
    /// </summary>
    public string? PrimaryNetworkInterface
    {
        get;
        set;
    }

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

    /// <summary>
    /// Number of received packets containing errors.
    /// </summary>
    public long NetworkReceiveErrors
    {
        get;
        set;
    }

    /// <summary>
    /// Number of transmitted packets containing errors.
    /// </summary>
    public long NetworkSendErrors
    {
        get;
        set;
    }

    /// <summary>
    /// Number of received packets discarded.
    /// </summary>
    public long NetworkReceiveDiscards
    {
        get;
        set;
    }

    /// <summary>
    /// Number of transmitted packets discarded.
    /// </summary>
    public long NetworkSendDiscards
    {
        get;
        set;
    }

    /// <summary>
    /// TCP retransmissions per second.
    /// </summary>
    public decimal TcpRetransmissionsPerSec
    {
        get;
        set;
    }
}