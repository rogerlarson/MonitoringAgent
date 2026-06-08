// ============================================================================
// Project: MonitoringAgent.Api
// File: IgnitionMetricsResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents current Ignition service and process metrics for a
//      monitored server.
//
//      Used by server detail views to display the latest Ignition health,
//      version information, resource utilization, and runtime statistics.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents current Ignition monitoring metrics.
/// </summary>
public sealed class IgnitionMetricsResponse
{
    // =====================================================================
    // Service Status
    // =====================================================================

    /// <summary>
    /// Indicates whether the Ignition JVM process is running.
    /// </summary>
    public bool ProcessRunning
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the Ignition Windows service is running.
    /// </summary>
    public bool ServiceRunning
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

    // =====================================================================
    // Runtime Information
    // =====================================================================

    /// <summary>
    /// Ignition uptime in minutes.
    /// </summary>
    public long UptimeMinutes
    {
        get;
        set;
    }

    // =====================================================================
    // Extended Metrics
    // =====================================================================

    /// <summary>
    /// Gateway response time in milliseconds.
    /// </summary>
    public int GatewayResponseMs
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition memory usage in megabytes.
    /// </summary>
    public long IgnitionMemoryMb
    {
        get;
        set;
    }

    /// <summary>
    /// Ignition CPU utilization percentage.
    /// </summary>
    public decimal IgnitionCpuPercent
    {
        get;
        set;
    }
}