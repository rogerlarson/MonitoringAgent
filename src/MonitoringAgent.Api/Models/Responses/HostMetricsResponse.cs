// ============================================================================
// Project: MonitoringAgent.Api
// File: HostMetricsResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents current host performance metrics for a monitored server.
//
//      Used by server detail views to display the latest CPU, memory,
//      disk, and uptime information.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents current host performance metrics.
/// </summary>
public sealed class HostMetricsResponse
{
    // =====================================================================
    // CPU & Memory
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

    // =====================================================================
    // Disk
    // =====================================================================

    /// <summary>
    /// Percentage of disk space currently in use.
    /// </summary>
    public decimal DiskPercentUsed
    {
        get;
        set;
    }

    // =====================================================================
    // System
    // =====================================================================

    /// <summary>
    /// System uptime in minutes.
    /// </summary>
    public long SystemUptimeMinutes
    {
        get;
        set;
    }
}