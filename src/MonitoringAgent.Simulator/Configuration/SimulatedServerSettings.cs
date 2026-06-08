/*
===============================================================================
SimulatedServerSettings
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Represents a simulated server configuration record loaded from
application configuration.

Responsibilities:
- Define the simulated server name
- Define the simulation profile
- Provide configuration input to the simulator worker

Configuration Source:
appsettings.json

Example:

{
  "ServerName": "POLARIS",
  "Profile": "Healthy"
}

Notes:
This class represents configuration data only.

During simulator startup these records are converted into
runtime SimulatedServer instances used by the simulation engine.

===============================================================================
*/

namespace MonitoringAgent.Simulator.Configuration;

/// <summary>
/// Individual simulated server configuration.
/// </summary>
public sealed class SimulatedServerSettings
{
    /// <summary>
    /// Unique server name used within the simulator.
    ///
    /// This value is included in generated snapshots
    /// and displayed throughout the Monitoring Platform.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    }
    = string.Empty;

    /// <summary>
    /// Simulation profile name.
    ///
    /// Supported values:
    ///
    /// - Healthy
    /// - HighCpu
    /// - HighMemory
    /// - DiskFull
    /// - GatewayDown
    /// - IgnitionDown
    /// - Offline
    ///
    /// This value is converted into a
    /// SimulationProfile enum during startup.
    /// </summary>
    public string Profile
    {
        get;
        set;
    }
    = "Healthy";
}