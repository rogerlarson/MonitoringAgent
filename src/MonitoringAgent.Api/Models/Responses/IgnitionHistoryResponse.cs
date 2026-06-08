// ============================================================================
// Project: MonitoringAgent.Api
// File: IgnitionHistoryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a historical Ignition monitoring data point.
//
//      Used by dashboard charts and service monitoring views to display
//      Ignition process health, resource utilization, and uptime trends.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a historical Ignition snapshot.
/// </summary>
public sealed class IgnitionHistoryResponse
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
    // Service Status
    // =====================================================================

    /// <summary>
    /// Indicates whether the Ignition Windows service was running.
    /// </summary>
    public bool ServiceRunning
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the Ignition JVM process was running.
    /// </summary>
    public bool ProcessRunning
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
    /// Number of JVM threads.
    /// </summary>
    public int ThreadCount
    {
        get;
        set;
    }

    /// <summary>
    /// Number of process handles.
    /// </summary>
    public int HandleCount
    {
        get;
        set;
    }

    // =====================================================================
    // Process Information
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
    /// Process identifier (PID).
    /// </summary>
    public int ProcessId
    {
        get;
        set;
    }
}