// ============================================================================
// Project: MonitoringAgent.Api
// File: HostSnapshotHistoryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a historical host snapshot data point.
//
//      Used by dashboard charts and trend views to display CPU, memory,
//      and disk utilization over time.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a historical host snapshot.
/// </summary>
public sealed class HostSnapshotHistoryResponse
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
}