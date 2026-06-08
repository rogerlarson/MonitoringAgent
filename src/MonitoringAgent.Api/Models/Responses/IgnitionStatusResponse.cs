// ============================================================================
// Project: MonitoringAgent.Api
// File: IgnitionStatusResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the current status of a monitored Ignition service.
//
//      Used by service detail views to display the latest Ignition process
//      health, version information, resource utilization, and runtime data.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents the current Ignition service status.
/// </summary>
public sealed class IgnitionStatusResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Identifier of the monitored server service.
    /// </summary>
    public int ServerServiceId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the monitored service.
    /// </summary>
    public string ServiceName
    {
        get;
        set;
    }
        = string.Empty;

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
    // Status
    // =====================================================================

    /// <summary>
    /// Current service status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = "Unknown";

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
    /// Java runtime version used by Ignition.
    /// </summary>
    public string? JavaVersion
    {
        get;
        set;
    }
}