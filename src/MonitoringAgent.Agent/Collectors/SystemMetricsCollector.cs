// ============================================================================
// Project: MonitoringAgent.Agent
// File: SystemMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects operating system metrics from the local machine.
//
//      System metrics include process count and system uptime information
//      used for monitoring, alerting, and health reporting.
// ============================================================================

using System.Diagnostics;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects operating system metrics.
/// </summary>
public sealed class SystemMetricsCollector
{
    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates operating system metrics on the supplied snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// Snapshot being populated.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Completed task.
    /// </returns>
    public Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        snapshot.ProcessCount =
            Process.GetProcesses().Length;

        snapshot.SystemUptimeMinutes =
            GetSystemUptimeMinutes();

        return Task.CompletedTask;
    }

    // =====================================================================
    // System Information Helpers
    // =====================================================================

    /// <summary>
    /// Returns the current system uptime in minutes.
    /// </summary>
    /// <returns>
    /// Total system uptime in minutes.
    /// </returns>
    private static long GetSystemUptimeMinutes()
    {
        return Environment.TickCount64
            / 1000
            / 60;
    }
}