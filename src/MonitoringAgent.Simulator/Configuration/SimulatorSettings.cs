/*
===============================================================================
SimulatorSettings
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Stores global configuration for the MonitoringAgent Simulator.

Responsibilities:
- Define Monitoring API connection settings
- Configure snapshot generation frequency
- Define simulated server inventory
- Provide runtime configuration to the simulator worker

Configuration Source:
appsettings.json

Example:

{
  "Simulator": {
    "ApiUrl": "https://localhost:7216",
    "ApiKey": "optional-key",
    "SnapshotIntervalSeconds": 15,
    "Servers": [
      {
        "ServerName": "POLARIS",
        "Profile": "Healthy"
      }
    ]
  }
}

Notes:
Configuration is loaded during application startup and bound
using the Microsoft Options pattern.

===============================================================================
*/

namespace MonitoringAgent.Simulator.Configuration;

/// <summary>
/// Global simulator configuration.
/// </summary>
public sealed class SimulatorSettings
{
    /// <summary>
    /// Base URL of the MonitoringAgent API.
    ///
    /// Example:
    /// https://localhost:7216
    /// </summary>
    public string ApiUrl
    {
        get;
        set;
    }
    = string.Empty;

    /// <summary>
    /// Optional API key used when the
    /// Monitoring API requires authentication.
    /// </summary>
    public string ApiKey
    {
        get;
        set;
    }
    = string.Empty;

    /// <summary>
    /// Frequency, in seconds, at which
    /// snapshots are generated and submitted.
    ///
    /// Example:
    /// 15 = one snapshot every 15 seconds
    /// </summary>
    public int SnapshotIntervalSeconds
    {
        get;
        set;
    }
    = 15;

    /// <summary>
    /// Collection of simulated servers that
    /// will participate in snapshot generation.
    /// </summary>
    public List<SimulatedServerSettings>
        Servers
    {
        get;
        set;
    }
    = [];
}