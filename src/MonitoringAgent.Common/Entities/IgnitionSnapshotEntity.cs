// ============================================================================
// Project: MonitoringAgent
// File: IgnitionSnapshotEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a point-in-time Ignition gateway health snapshot
//      collected from a monitored service.
//
//      Ignition snapshots capture service availability, process health,
//      resource utilization, version information, and runtime statistics
//      used for monitoring, alerting, and historical analysis.
// ============================================================================

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a point-in-time Ignition health snapshot collected from a
/// monitored service.
/// </summary>
public sealed class IgnitionSnapshotEntity
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the Ignition snapshot.
    /// </summary>
    public long IgnitionSnapshotId
    {
        get;
        set;
    }

    /// <summary>
    /// Associated monitored service identifier.
    /// </summary>
    public int ServerServiceId
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
    // Service Status
    // =====================================================================

    /// <summary>
    /// Indicates whether the Ignition Windows service is running.
    /// </summary>
    public bool ServiceRunning
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the Ignition JVM process is running.
    /// </summary>
    public bool ProcessRunning
    {
        get;
        set;
    }

    // =====================================================================
    // Version Information
    // =====================================================================

    /// <summary>
    /// Installed Ignition version.
    /// </summary>
    public string? IgnitionVersion
    {
        get;
        set;
    }

    /// <summary>
    /// JVM version used by the Ignition process.
    /// </summary>
    public string? JavaVersion
    {
        get;
        set;
    }

    // =====================================================================
    // Resource Utilization
    // =====================================================================

    /// <summary>
    /// Ignition CPU utilization percentage.
    /// </summary>
    public decimal CpuPercent
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition memory usage in megabytes.
    /// </summary>
    public long MemoryMb
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition JVM thread count.
    /// </summary>
    public int ThreadCount
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition process handle count.
    /// </summary>
    public int HandleCount
    {
        get;
        set;
    }

    // =====================================================================
    // Runtime Information
    // =====================================================================

    /// <summary>
    /// Ignition process uptime in minutes.
    /// </summary>
    public long UptimeMinutes
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition process identifier (PID).
    /// </summary>
    public int ProcessId
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition process executable name.
    /// </summary>
    public string? ProcessName
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
    /// Associated monitored service.
    /// </summary>
    public ServerServiceEntity ServerService
    {
        get;
        set;
    } = null!;
}