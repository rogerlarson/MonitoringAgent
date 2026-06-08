/*
===============================================================================
ServerStatusCalculator
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Calculates the overall health status of a monitored server
using the most recent monitoring snapshots.

Responsibilities:
- Evaluate host resource utilization
- Evaluate gateway availability
- Evaluate Ignition health
- Determine overall server status
- Apply status priority rules

Inputs:
- HostSnapshotEntity
- GatewaySnapshotEntity
- IgnitionSnapshotEntity

Possible Results:
- Unknown
- Healthy
- Warning
- Critical

Offline Handling:
Offline determination is performed by
HostOfflineMonitorWorker before this calculator
is invoked.

Status Priority:

    Offline
        ↓
    Critical
        ↓
    Warning
        ↓
    Healthy
        ↓
    Unknown

Notes:
This calculator determines the server health status
displayed throughout the MonitoringAgent platform.

Dashboard indicators, server lists, and summary metrics
all rely on the status returned by this calculator.

===============================================================================
*/

using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;

namespace MonitoringAgent.Engine.Calculators;

/// <summary>
/// Calculates the overall server health status based on:
///
/// - Host metrics
/// - Gateway connectivity
/// - Ignition health
/// - Heartbeat recency
///
/// Status Priority:
///
/// Offline
///     ↓
/// Critical
///     ↓
/// Warning
///     ↓
/// Healthy
///     ↓
/// Unknown
/// </summary>
public static class ServerStatusCalculator
{
    /// <summary>
    /// Calculates the current server status.
    /// </summary>
    public static ServerStatus Calculate(
        HostSnapshotEntity? hostSnapshot,
        GatewaySnapshotEntity? gatewaySnapshot,
        IgnitionSnapshotEntity? ignitionSnapshot)
    {

        // -------------------------------------------------------------------------
        // Gateway Health Evaluation
        // -------------------------------------------------------------------------

        //
        // Gateway unreachable.
        //
        if (gatewaySnapshot != null &&
            !gatewaySnapshot.Reachable)
        {
            return ServerStatus.Critical;
        }

        // -------------------------------------------------------------------------
        // Ignition Health Evaluation
        // -------------------------------------------------------------------------

        //
        // Ignition service/process failure.
        //
        if (ignitionSnapshot != null)
        {
            if (!ignitionSnapshot.ServiceRunning ||
                !ignitionSnapshot.ProcessRunning)
            {
                return ServerStatus.Critical;
            }
        }

        // -------------------------------------------------------------------------
        // Snapshot Availability Validation
        // -------------------------------------------------------------------------

        //
        // No host snapshot available.
        //
        if (hostSnapshot == null)
        {
            return ServerStatus.Unknown;
        }

        // -------------------------------------------------------------------------
        // Critical Conditions
        // -------------------------------------------------------------------------
        //
        // Any critical condition immediately results
        // in Critical server status.
        //

        if (hostSnapshot.DiskPercentUsed >= 95)
        {
            return ServerStatus.Critical;
        }

        if (hostSnapshot.MemoryPercent >= 95)
        {
            return ServerStatus.Critical;
        }

        if (hostSnapshot.CpuPercent >= 95)
        {
            return ServerStatus.Critical;
        }

        if (hostSnapshot.DiskReadLatencyMs >= 250 ||
            hostSnapshot.DiskWriteLatencyMs >= 250)
        {
            return ServerStatus.Critical;
        }

        if (hostSnapshot.AvailableMemoryMb <= 512)
        {
            return ServerStatus.Critical;
        }

        if (hostSnapshot.AvgDiskQueueLength >= 50)
        {
            return ServerStatus.Critical;
        }

        // -------------------------------------------------------------------------
        // Warning Conditions
        // -------------------------------------------------------------------------
        //
        // Warning conditions are evaluated only after
        // all critical conditions have been ruled out.
        //

        if (hostSnapshot.DiskPercentUsed >= 90)
        {
            return ServerStatus.Warning;
        }

        if (hostSnapshot.MemoryPercent >= 85)
        {
            return ServerStatus.Warning;
        }

        if (hostSnapshot.CpuPercent >= 85)
        {
            return ServerStatus.Warning;
        }

        if (hostSnapshot.DiskReadLatencyMs >= 50 ||
            hostSnapshot.DiskWriteLatencyMs >= 50)
        {
            return ServerStatus.Warning;
        }

        if (hostSnapshot.AvgDiskQueueLength >= 10)
        {
            return ServerStatus.Warning;
        }

        if (hostSnapshot.AvailableMemoryMb <= 2048)
        {
            return ServerStatus.Warning;
        }

        // -------------------------------------------------------------------------
        // Healthy Status
        // -------------------------------------------------------------------------
        //
        // No warning or critical conditions detected.
        //
        return ServerStatus.Healthy;
    }
}