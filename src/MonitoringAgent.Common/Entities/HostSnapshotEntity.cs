// ============================================================================
// Project: MonitoringAgent
// File: HostSnapshotEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a point-in-time host health snapshot collected from a
//      monitored server.
//
//      Host snapshots capture operating system, processor, memory, disk,
//      and network performance metrics used for monitoring, alerting,
//      reporting, and historical trend analysis.
// ============================================================================

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a point-in-time host health snapshot collected from a
/// monitored server.
/// </summary>
public sealed class HostSnapshotEntity
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the snapshot.
    /// </summary>
    public long SnapshotId
    {
        get;
        set;
    }

    /// <summary>
    /// Associated server identifier.
    /// </summary>
    public int ServerId
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the snapshot was collected.
    /// </summary>
    public DateTime SnapshotUtc
    {
        get;
        set;
    }

    // =====================================================================
    // System Metrics
    // =====================================================================

    /// <summary>
    /// Total CPU utilization percentage.
    /// </summary>
    public decimal CpuPercent
    {
        get;
        set;
    }

    /// <summary>
    /// Total memory utilization percentage.
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
    /// Total process count.
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
    // Disk Metrics
    // =====================================================================

    /// <summary>
    /// System drive being monitored.
    /// </summary>
    public string? SystemDrive
    {
        get;
        set;
    }

    /// <summary>
    /// Percentage of disk space currently used.
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
    /// Rolling average disk queue length.
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
    /// Primary network adapter name.
    /// </summary>
    public string? PrimaryNetworkInterface
    {
        get;
        set;
    }

    /// <summary>
    /// Bytes received per second.
    /// </summary>
    public decimal NetworkBytesReceivedPerSec
    {
        get;
        set;
    }

    /// <summary>
    /// Bytes sent per second.
    /// </summary>
    public decimal NetworkBytesSentPerSec
    {
        get;
        set;
    }

    /// <summary>
    /// Total receive errors.
    /// </summary>
    public long NetworkReceiveErrors
    {
        get;
        set;
    }

    /// <summary>
    /// Total send errors.
    /// </summary>
    public long NetworkSendErrors
    {
        get;
        set;
    }

    /// <summary>
    /// Total receive discards.
    /// </summary>
    public long NetworkReceiveDiscards
    {
        get;
        set;
    }

    /// <summary>
    /// Total send discards.
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

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the snapshot record was created.
    /// </summary>
    public DateTime CreatedDateUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Navigation Properties
    // =====================================================================

    /// <summary>
    /// Associated monitored server.
    /// </summary>
    public ServerEntity Server
    {
        get;
        set;
    } = null!;
}