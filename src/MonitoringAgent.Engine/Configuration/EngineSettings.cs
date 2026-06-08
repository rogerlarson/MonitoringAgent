/*
===============================================================================
EngineSettings
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Stores scheduling configuration for MonitoringAgent
background workers.

Responsibilities:
- Configure worker execution intervals
- Control maintenance task frequency
- Control alert evaluation frequency
- Control offline detection frequency

Configuration Source:
appsettings.json

Example:

{
  "Engine": {
    "OfflineMonitorIntervalSeconds": 15,
    "AlertMonitorIntervalSeconds": 15,
    "SnapshotCleanupIntervalMinutes": 60,
    "LogCleanupIntervalHours": 24
  }
}

Workers:
- HostOfflineMonitorWorker
- SnapshotAlertWorker
- SnapshotCleanupWorker
- LogCleanupWorker

Notes:
All values represent execution intervals.

Lower values provide more responsive monitoring
but increase database and CPU activity.

===============================================================================
*/

namespace MonitoringAgent.Engine.Configuration;

/// <summary>
/// Engine worker scheduling configuration.
/// </summary>
public sealed class EngineSettings
{
    /// <summary>
    /// Frequency, in seconds, at which
    /// HostOfflineMonitorWorker evaluates
    /// server heartbeat status.
    /// </summary>
    public int OfflineMonitorIntervalSeconds
    {
        get;
        set;
    }
    = 15;

    /// <summary>
    /// Frequency, in seconds, at which
    /// SnapshotAlertWorker evaluates
    /// monitoring snapshots and alert rules.
    /// </summary>
    public int AlertMonitorIntervalSeconds
    {
        get;
        set;
    }
    = 15;

    /// <summary>
    /// Frequency, in minutes, at which
    /// SnapshotCleanupWorker executes
    /// retention cleanup operations.
    /// </summary>
    public int SnapshotCleanupIntervalMinutes
    {
        get;
        set;
    }
    = 60;

    /// <summary>
    /// Frequency, in hours, at which
    /// LogCleanupWorker removes expired
    /// log files.
    /// </summary>
    public int LogCleanupIntervalHours
    {
        get;
        set;
    }
    = 24;
}