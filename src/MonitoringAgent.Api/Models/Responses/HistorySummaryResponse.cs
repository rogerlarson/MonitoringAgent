// ============================================================================
// Project: MonitoringAgent.Api
// File: HistorySummaryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents aggregated historical performance statistics for a server.
//
//      Used by dashboards and reporting views to display averages, peaks,
//      and performance trends over a selected time range.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents summarized historical performance metrics.
/// </summary>
public sealed class HistorySummaryResponse
{
    // =====================================================================
    // CPU Metrics
    // =====================================================================

    /// <summary>
    /// Average CPU utilization percentage.
    /// </summary>
    public decimal CpuAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum CPU utilization percentage.
    /// </summary>
    public decimal CpuMaximum
    {
        get;
        set;
    }

    // =====================================================================
    // Memory Metrics
    // =====================================================================

    /// <summary>
    /// Average memory utilization percentage.
    /// </summary>
    public decimal MemoryAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum memory utilization percentage.
    /// </summary>
    public decimal MemoryMaximum
    {
        get;
        set;
    }

    // =====================================================================
    // Disk Utilization Metrics
    // =====================================================================

    /// <summary>
    /// Average disk utilization percentage.
    /// </summary>
    public decimal DiskAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum disk utilization percentage.
    /// </summary>
    public decimal DiskMaximum
    {
        get;
        set;
    }

    // =====================================================================
    // Disk Performance Metrics
    // =====================================================================

    /// <summary>
    /// Maximum disk read latency in milliseconds.
    /// </summary>
    public decimal DiskReadLatencyMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum disk write latency in milliseconds.
    /// </summary>
    public decimal DiskWriteLatencyMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum average disk queue length.
    /// </summary>
    public decimal AvgDiskQueueMaximum
    {
        get;
        set;
    }

    // =====================================================================
    // Network Metrics
    // =====================================================================

    /// <summary>
    /// Maximum inbound network throughput in bytes per second.
    /// </summary>
    public decimal NetworkInMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum outbound network throughput in bytes per second.
    /// </summary>
    public decimal NetworkOutMaximum
    {
        get;
        set;
    }
}