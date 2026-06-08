// ============================================================================
// Project: MonitoringAgent
// File: LogSettings.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines configuration settings used by the monitoring platform's
//      logging subsystem.
//
//      Controls log storage location, retention policies, and category-
//      specific logging behavior for monitoring activities.
// ============================================================================

namespace MonitoringAgent.Common.Configuration;

/// <summary>
/// Defines configuration settings used for application logging.
/// </summary>
public sealed class LogSettings
{
    // =====================================================================
    // Storage
    // =====================================================================

    /// <summary>
    /// Directory where log files are written.
    /// </summary>
    public string LogDirectory
    {
        get;
        set;
    } = "Logs";

    /// <summary>
    /// Number of days log files are retained before cleanup.
    /// </summary>
    public int RetentionDays
    {
        get;
        set;
    } = 30;

    // =====================================================================
    // Category Logging
    // =====================================================================

    /// <summary>
    /// Indicates whether API activity logging is enabled.
    /// </summary>
    public bool EnableApiLogging
    {
        get;
        set;
    } = true;

    /// <summary>
    /// Indicates whether heartbeat activity logging is enabled.
    /// </summary>
    public bool EnableHeartbeatLogging
    {
        get;
        set;
    } = true;

    /// <summary>
    /// Indicates whether alert activity logging is enabled.
    /// </summary>
    public bool EnableAlertLogging
    {
        get;
        set;
    } = true;

    /// <summary>
    /// Indicates whether email activity logging is enabled.
    /// </summary>
    public bool EnableEmailLogging
    {
        get;
        set;
    } = true;

    /// <summary>
    /// Indicates whether maintenance activity logging is enabled.
    /// </summary>
    public bool EnableMaintenanceLogging
    {
        get;
        set;
    } = true;
}