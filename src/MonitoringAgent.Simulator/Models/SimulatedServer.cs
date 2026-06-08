/*
===============================================================================
SimulatedServer
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Represents a runtime simulated server instance used by the
MonitoringAgent Simulator.

Responsibilities:
- Store the simulated server name
- Store the active simulation profile
- Provide input to SnapshotGenerator

Notes:
Instances are created from SimulatorSettings during
simulator startup and remain in memory for the lifetime
of the simulator worker.

===============================================================================
*/

using MonitoringAgent.Simulator.Enums;

namespace MonitoringAgent.Simulator.Models;

/// <summary>
/// Represents a runtime simulated server and its
/// associated simulation profile.
/// </summary>
public sealed class SimulatedServer
{
    /// <summary>
    /// Display name of the simulated server.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    }
    = string.Empty;

    /// <summary>
    /// Active simulation profile used to determine
    /// generated monitoring behavior.
    /// </summary>
    public SimulationProfile Profile
    {
        get;
        set;
    }
}