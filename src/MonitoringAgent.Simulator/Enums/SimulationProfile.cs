/*
===============================================================================
SimulationProfile
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Defines the available simulation modes supported by the
MonitoringAgent Simulator.

Each profile modifies generated monitoring data in order
to simulate specific infrastructure conditions and alert
scenarios.

Used By:
- SimulatorSettings
- SimulatedServer
- SnapshotGenerator
- Worker

Notes:
Profiles are intended to validate dashboard behavior,
alert generation, alert recovery, and notification logic.

===============================================================================
*/

namespace MonitoringAgent.Simulator.Enums;

/// <summary>
/// Defines the available simulator profiles.
/// </summary>
public enum SimulationProfile
{
    /// <summary>
    /// Normal operating conditions.
    /// All metrics remain within expected ranges.
    /// </summary>
    Healthy,

    /// <summary>
    /// Simulates sustained high CPU utilization.
    /// </summary>
    HighCpu,

    /// <summary>
    /// Simulates memory pressure and low available memory.
    /// </summary>
    HighMemory,

    /// <summary>
    /// Simulates critically low disk space.
    /// </summary>
    DiskFull,

    /// <summary>
    /// Simulates an unreachable Ignition gateway.
    /// </summary>
    GatewayDown,

    /// <summary>
    /// Simulates an Ignition service or process outage.
    /// </summary>
    IgnitionDown,

    /// <summary>
    /// Simulates an offline monitoring agent.
    ///
    /// No snapshots are generated or submitted.
    /// This allows testing of snapshot age and
    /// agent availability alert rules.
    /// </summary>
    Offline
}