// ============================================================================
// Project: MonitoringAgent
// File: ServerStatus.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the operational health state of a monitored server.
//
//      Status values are calculated automatically by the monitoring engine
//      using heartbeat activity, alert conditions, and collected health
//      metrics. These values are intended to reflect the current state of a
//      monitored server and should not be assigned directly by API clients.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Represents the current operational state of a monitored server.
/// </summary>
public enum ServerStatus
{
    /// <summary>
    /// Status has not yet been evaluated. Typically used for newly
    /// registered servers.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Server is online and all monitored metrics are operating within
    /// acceptable thresholds.
    /// </summary>
    Healthy = 1,

    /// <summary>
    /// Server is online but one or more monitored metrics exceed warning
    /// thresholds.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Server is online but one or more monitored metrics exceed critical
    /// thresholds.
    /// </summary>
    Critical = 3,

    /// <summary>
    /// Server has exceeded the configured heartbeat timeout and is
    /// considered unreachable.
    /// </summary>
    Offline = 4
}