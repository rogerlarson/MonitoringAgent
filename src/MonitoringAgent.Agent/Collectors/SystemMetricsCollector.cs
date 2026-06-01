using System.Diagnostics;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects operating system metrics.
/// </summary>
public sealed class SystemMetricsCollector
{
    /// <summary>
    /// Populates system metrics on the supplied snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// Snapshot to populate.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
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

    /// <summary>
    /// Returns the current system uptime in minutes.
    /// </summary>
    /// <returns>
    /// Total uptime in minutes.
    /// </returns>
    private static long GetSystemUptimeMinutes()
    {
        return Environment.TickCount64
            / 1000
            / 60;
    }
}