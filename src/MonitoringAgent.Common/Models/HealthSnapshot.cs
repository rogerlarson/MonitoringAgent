// ============================================================================
// Project: MonitoringAgent
// File: HealthSnapshot.cs
// Author: Roger Larson
// Date Created: 05/29/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a point-in-time health snapshot collected from a monitored
//      server and transmitted to the central monitoring API.
//
//      Captures operating system, hardware, network, storage, security,
//      gateway, and Ignition-specific metrics used for monitoring,
//      alerting, reporting, and historical analysis.
//
//      A new snapshot is typically collected on a scheduled interval by the
//      monitoring agent and submitted to the monitoring API for storage and
//      evaluation.
// ============================================================================

namespace MonitoringAgent.Common.Models;

/// <summary>
/// Represents a point-in-time health snapshot collected from a monitored
/// server.
/// </summary>
public sealed class HealthSnapshot
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for this snapshot.
    /// </summary>
    public Guid SnapshotId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Name of the monitored server.
    /// </summary>
    public string ServerName { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the snapshot was collected.
    /// </summary>
    public DateTime SnapshotUtc { get; set; }

    /// <summary>
    /// Monitoring agent version.
    /// </summary>
    public string AgentVersion { get; set; } = string.Empty;

    /// <summary>
    /// Windows domain name.
    /// </summary>
    public string DomainName { get; set; } = string.Empty;

    // =====================================================================
    // System
    // =====================================================================

    /// <summary>
    /// Total CPU utilization percentage.
    /// </summary>
    public decimal CpuPercent { get; set; }

    /// <summary>
    /// Total memory utilization percentage.
    /// </summary>
    public decimal MemoryPercent { get; set; }

    /// <summary>
    /// Available physical memory in megabytes.
    /// </summary>
    public long AvailableMemoryMb { get; set; }

    /// <summary>
    /// Total process count.
    /// </summary>
    public int ProcessCount { get; set; }

    /// <summary>
    /// System uptime in minutes.
    /// </summary>
    public long SystemUptimeMinutes { get; set; }

    /// <summary>
    /// Context switches per second.
    /// </summary>
    public decimal ContextSwitchesPerSec { get; set; }

    /// <summary>
    /// Page faults per second.
    /// </summary>
    public decimal PageFaultsPerSec { get; set; }

    // =====================================================================
    // Disk
    // =====================================================================

    /// <summary>
    /// System drive being monitored.
    /// </summary>
    public string SystemDrive { get; set; } = string.Empty;

    /// <summary>
    /// Percentage of disk space currently used.
    /// </summary>
    public decimal DiskPercentUsed { get; set; }

    /// <summary>
    /// Available disk space in gigabytes.
    /// </summary>
    public decimal DiskFreeGb { get; set; }

    /// <summary>
    /// Disk read operations per second.
    /// </summary>
    public decimal DiskReadsPerSec { get; set; }

    /// <summary>
    /// Disk write operations per second.
    /// </summary>
    public decimal DiskWritesPerSec { get; set; }

    /// <summary>
    /// Average disk read latency in milliseconds.
    /// </summary>
    public decimal DiskReadLatencyMs { get; set; }

    /// <summary>
    /// Average disk write latency in milliseconds.
    /// </summary>
    public decimal DiskWriteLatencyMs { get; set; }

    /// <summary>
    /// Current disk queue length.
    /// </summary>
    public decimal DiskQueueLength { get; set; }

    /// <summary>
    /// Rolling average disk queue length.
    /// </summary>
    public decimal AvgDiskQueueLength { get; set; }

    // =====================================================================
    // Network
    // =====================================================================

    /// <summary>
    /// Primary network adapter name.
    /// </summary>
    public string PrimaryNetworkInterface { get; set; } = string.Empty;

    /// <summary>
    /// Bytes received per second.
    /// </summary>
    public decimal NetworkBytesReceivedPerSec { get; set; }

    /// <summary>
    /// Bytes sent per second.
    /// </summary>
    public decimal NetworkBytesSentPerSec { get; set; }

    /// <summary>
    /// Total receive errors.
    /// </summary>
    public long NetworkReceiveErrors { get; set; }

    /// <summary>
    /// Total send errors.
    /// </summary>
    public long NetworkSendErrors { get; set; }

    /// <summary>
    /// Total receive discards.
    /// </summary>
    public long NetworkReceiveDiscards { get; set; }

    /// <summary>
    /// Total send discards.
    /// </summary>
    public long NetworkSendDiscards { get; set; }

    /// <summary>
    /// TCP retransmissions per second.
    /// </summary>
    public decimal TcpRetransmissionsPerSec { get; set; }

    // =====================================================================
    // Ignition
    // =====================================================================

    /// <summary>
    /// Indicates whether the Ignition Windows service is running.
    /// </summary>
    public bool IgnitionServiceRunning { get; set; }

    /// <summary>
    /// Indicates whether the Ignition JVM process is running.
    /// </summary>
    public bool IgnitionProcessRunning { get; set; }

    /// <summary>
    /// Ignition version.
    /// </summary>
    public string IgnitionVersion { get; set; } = string.Empty;

    /// <summary>
    /// JVM version used by the Ignition process.
    /// </summary>
    public string JavaVersion { get; set; } = string.Empty;

    /// <summary>
    /// Ignition CPU utilization percentage.
    /// </summary>
    public decimal IgnitionCpuPercent { get; set; }

    /// <summary>
    /// Ignition memory usage in megabytes.
    /// </summary>
    public long IgnitionMemoryMb { get; set; }

    /// <summary>
    /// Ignition JVM thread count.
    /// </summary>
    public int IgnitionThreadCount { get; set; }

    /// <summary>
    /// Ignition process handle count.
    /// </summary>
    public int IgnitionHandleCount { get; set; }

    /// <summary>
    /// Ignition uptime in minutes.
    /// </summary>
    public long IgnitionUptimeMinutes { get; set; }

    /// <summary>
    /// Ignition process ID (PID).
    /// </summary>
    public int IgnitionProcessId { get; set; }

    /// <summary>
    /// Ignition process executable name (java or javaw).
    /// </summary>
    public string IgnitionProcessName { get; set; } = string.Empty;

    // =====================================================================
    // Gateway
    // =====================================================================

    /// <summary>
    /// Indicates whether the gateway is reachable.
    /// </summary>
    public bool GatewayReachable { get; set; }

    /// <summary>
    /// HTTP status code returned by the gateway.
    /// </summary>
    public int GatewayHttpStatusCode { get; set; }

    /// <summary>
    /// Gateway response time in milliseconds.
    /// </summary>
    public long GatewayResponseMs { get; set; }

    // =====================================================================
    // Security
    // =====================================================================

    /// <summary>
    /// Failed login attempts detected during the previous hour.
    /// </summary>
    public int FailedLoginsLastHour { get; set; }

    /// <summary>
    /// Privilege escalation events detected during the previous hour.
    /// </summary>
    public int PrivilegeEscalationsLastHour { get; set; }

    /// <summary>
    /// Critical Windows event log entries detected during the previous hour.
    /// </summary>
    public int CriticalEventLogEntriesLastHour { get; set; }

    // =====================================================================
    // Host Information
    // =====================================================================

    /// <summary>
    /// Operating system name.
    /// </summary>
    public string OperatingSystem { get; set; } = string.Empty;

    /// <summary>
    /// Operating system version.
    /// </summary>
    public string OperatingSystemVersion { get; set; } = string.Empty;

    /// <summary>
    /// Number of logical processors.
    /// </summary>
    public int ProcessorCount { get; set; }

    /// <summary>
    /// Total installed memory in megabytes.
    /// </summary>
    public long TotalMemoryMb { get; set; }
}