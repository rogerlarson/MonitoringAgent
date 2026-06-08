// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerHealthSummaryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a summary of server health status counts.
//
//      Used by dashboard and reporting views to display the distribution
//      of monitored servers across health states.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents summarized server health information.
/// </summary>
public sealed class ServerHealthSummaryResponse
{
    // =====================================================================
    // Health Status Counts
    // =====================================================================

    /// <summary>
    /// Number of servers currently reporting a healthy status.
    /// </summary>
    public int Healthy
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently reporting a warning status.
    /// </summary>
    public int Warning
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently reporting a critical status.
    /// </summary>
    public int Critical
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently considered offline.
    /// </summary>
    public int Offline
    {
        get;
        set;
    }
}