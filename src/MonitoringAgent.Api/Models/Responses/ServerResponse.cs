// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a summarized server record returned by server listing
//      endpoints.
//
//      Provides current server health information and key performance
//      indicators for dashboards and server overview pages.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a summarized monitored server.
/// </summary>
public sealed class ServerResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the server.
    /// </summary>
    public int ServerId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the monitored server.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Status
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the server last reported data.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Current calculated server status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = "Unknown";

    // =====================================================================
    // Performance Metrics
    // =====================================================================

    /// <summary>
    /// Latest CPU utilization percentage.
    /// </summary>
    public decimal? CpuPercent
    {
        get;
        set;
    }

    /// <summary>
    /// Latest memory utilization percentage.
    /// </summary>
    public decimal? MemoryPercent
    {
        get;
        set;
    }

    /// <summary>
    /// Latest disk utilization percentage.
    /// </summary>
    public decimal? DiskPercentUsed
    {
        get;
        set;
    }

    // =====================================================================
    // Service Health
    // =====================================================================

    /// <summary>
    /// Indicates whether the monitored gateway is reachable.
    /// </summary>
    public bool? GatewayReachable
    {
        get;
        set;
    }
}