/*
===============================================================================
MonitoringSettings
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Stores monitoring behavior settings used by the
MonitoringAgent Engine.

Responsibilities:
- Define server offline thresholds
- Control heartbeat monitoring behavior
- Configure server availability calculations

Configuration Source:
appsettings.json

Example:

{
  "Monitoring": {
    "OfflineThresholdMinutes": 2
  }
}

Used By:
- HostOfflineMonitorWorker
- AlertService
- Future availability calculations

Notes:
A server is considered offline when the time elapsed
since the last reported snapshot exceeds the configured
offline threshold.

Lower values provide faster outage detection but may
increase sensitivity to temporary network interruptions.

===============================================================================
*/

namespace MonitoringAgent.Engine.Configuration;

/// <summary>
/// Monitoring behavior configuration.
/// </summary>
public sealed class MonitoringSettings
{
    /// <summary>
    /// Number of minutes a server may go
    /// without reporting before being
    /// considered offline.
    ///
    /// Example:
    /// LastSeenUtc = 10:00
    /// Threshold = 2 minutes
    ///
    /// At 10:02 or later:
    /// ServerStatus = Offline
    /// </summary>
    public int OfflineThresholdMinutes
    {
        get;
        set;
    }
    = 2;
}