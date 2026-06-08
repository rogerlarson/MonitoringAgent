// ============================================================================
// Project: MonitoringAgent.Api
// File: DashboardSummaryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents high-level monitoring statistics displayed on the
//      dashboard overview page.
//
//      Provides a summary of server health and active alert counts.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents dashboard summary information.
/// </summary>
public sealed class DashboardSummaryResponse
{
    // =====================================================================
    // Server Statistics
    // =====================================================================

    /// <summary>
    /// Total number of monitored servers.
    /// </summary>
    public int ServerCount
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently online.
    /// </summary>
    public int OnlineServers
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently offline.
    /// </summary>
    public int OfflineServers
    {
        get;
        set;
    }

    // =====================================================================
    // Alert Statistics
    // =====================================================================

    /// <summary>
    /// Number of active alerts.
    /// </summary>
    public int OpenAlerts
    {
        get;
        set;
    }

    /// <summary>
    /// Number of active critical alerts.
    /// </summary>
    public int CriticalAlerts
    {
        get;
        set;
    }

    /// <summary>
    /// Number of active warning alerts.
    /// </summary>
    public int WarningAlerts
    {
        get;
        set;
    }
}