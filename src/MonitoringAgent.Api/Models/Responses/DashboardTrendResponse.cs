// ============================================================================
// Project: MonitoringAgent.Api
// File: DashboardTrendResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents dashboard trend and health summary information.
//
//      Provides aggregate system statistics, alert activity, worker health,
//      and server health indicators used by the dashboard overview page.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents dashboard trend and health summary data.
/// </summary>
public sealed class DashboardTrendResponse
{
    // =====================================================================
    // Performance Metrics
    // =====================================================================

    /// <summary>
    /// Average CPU utilization across monitored servers.
    /// </summary>
    public decimal CpuAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum CPU utilization observed.
    /// </summary>
    public decimal CpuMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Average memory utilization across monitored servers.
    /// </summary>
    public decimal MemoryAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum memory utilization observed.
    /// </summary>
    public decimal MemoryMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Average disk utilization across monitored servers.
    /// </summary>
    public decimal DiskAverage
    {
        get;
        set;
    }

    /// <summary>
    /// Maximum disk utilization observed.
    /// </summary>
    public decimal DiskMaximum
    {
        get;
        set;
    }

    /// <summary>
    /// Average gateway response time in milliseconds.
    /// </summary>
    public decimal GatewayResponseAverageMs
    {
        get;
        set;
    }

    // =====================================================================
    // Alert Activity
    // =====================================================================

    /// <summary>
    /// Total number of alerts opened during the reporting period.
    /// </summary>
    public int TotalAlertsOpened
    {
        get;
        set;
    }

    /// <summary>
    /// Number of critical alerts opened during the reporting period.
    /// </summary>
    public int CriticalAlertsOpened
    {
        get;
        set;
    }

    /// <summary>
    /// Number of warning alerts opened during the reporting period.
    /// </summary>
    public int WarningAlertsOpened
    {
        get;
        set;
    }

    /// <summary>
    /// Number of currently active alerts.
    /// </summary>
    public int OpenAlerts
    {
        get;
        set;
    }

    /// <summary>
    /// Number of currently active critical alerts.
    /// </summary>
    public int CriticalAlerts
    {
        get;
        set;
    }

    // =====================================================================
    // Server Health
    // =====================================================================

    /// <summary>
    /// Number of servers currently reporting a healthy status.
    /// </summary>
    public int HealthyServers
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently reporting a warning status.
    /// </summary>
    public int WarningServers
    {
        get;
        set;
    }

    /// <summary>
    /// Number of servers currently reporting a critical status.
    /// </summary>
    public int CriticalServers
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
    // Engine Health
    // =====================================================================

    /// <summary>
    /// Number of monitoring workers currently running.
    /// </summary>
    public int RunningWorkers
    {
        get;
        set;
    }

    /// <summary>
    /// Number of monitoring workers currently stopped.
    /// </summary>
    public int StoppedWorkers
    {
        get;
        set;
    }

    // =====================================================================
    // Problem Servers
    // =====================================================================

    /// <summary>
    /// Servers with the highest number of active issues.
    /// </summary>
    public List<TopProblemServerResponse>
        TopProblemServers
    {
        get;
        set;
    }
        = [];
}