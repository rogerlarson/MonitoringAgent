/*
===============================================================================
RetentionSettings
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Defines data retention policies for monitoring data stored
within the MonitoringAgent database.

Responsibilities:
- Control host snapshot retention
- Control gateway snapshot retention
- Control Ignition snapshot retention
- Control alert event retention

Configuration Source:
appsettings.json

Example:

{
  "Retention": {
    "HostSnapshotDays": 30,
    "GatewaySnapshotDays": 30,
    "IgnitionSnapshotDays": 30,
    "AlertEventDays": 365
  }
}

Used By:
- SnapshotCleanupWorker

Notes:
Retention periods determine how long historical monitoring
data remains available before automatic cleanup occurs.

Longer retention periods provide greater historical visibility
but increase database storage requirements.

===============================================================================
*/

namespace MonitoringAgent.Engine.Configuration;

/// <summary>
/// Database retention policy configuration.
/// </summary>
public sealed class RetentionSettings
{
    /// <summary>
    /// Number of days host performance
    /// snapshots should be retained.
    ///
    /// Examples:
    /// - CPU utilization
    /// - Memory utilization
    /// - Disk utilization
    /// - Network metrics
    /// </summary>
    public int HostSnapshotDays
    {
        get;
        set;
    }
    = 30;

    /// <summary>
    /// Number of days gateway monitoring
    /// snapshots should be retained.
    ///
    /// Examples:
    /// - Reachability
    /// - HTTP status
    /// - Response times
    /// </summary>
    public int GatewaySnapshotDays
    {
        get;
        set;
    }
    = 30;

    /// <summary>
    /// Number of days Ignition monitoring
    /// snapshots should be retained.
    ///
    /// Examples:
    /// - Process status
    /// - Memory utilization
    /// - Thread counts
    /// - Handle counts
    /// </summary>
    public int IgnitionSnapshotDays
    {
        get;
        set;
    }
    = 30;

    /// <summary>
    /// Number of days alert events should
    /// be retained before cleanup.
    ///
    /// Includes:
    /// - Open alerts
    /// - Closed alerts
    /// - Acknowledged alerts
    /// - Suppressed alerts
    /// </summary>
    public int AlertEventDays
    {
        get;
        set;
    }
    = 365;
}