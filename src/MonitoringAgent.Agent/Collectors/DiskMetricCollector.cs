// ============================================================================
// Project: MonitoringAgent.Agent
// File: DiskMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects disk capacity and utilization metrics from the local
//      machine.
//
//      Retrieves available disk space and utilization statistics for the
//      configured monitoring drive and populates the health snapshot with
//      the collected values.
// ============================================================================

using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects disk capacity and utilization metrics.
/// </summary>
public sealed class DiskMetricsCollector
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly AgentSettings _settings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    public DiskMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings =
            settings.Value;
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates disk utilization metrics on the supplied snapshot.
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
        try
        {
            var drive =
                DriveInfo.GetDrives()
                    .FirstOrDefault(
                        drive =>
                            drive.IsReady &&
                            drive.Name.StartsWith(
                                _settings.MonitoredDrive,
                                StringComparison.OrdinalIgnoreCase));

            if (drive == null)
            {
                return Task.CompletedTask;
            }

            snapshot.SystemDrive =
                drive.Name;

            snapshot.DiskFreeGb =
                Math.Round(
                    drive.AvailableFreeSpace
                    / 1024m
                    / 1024m
                    / 1024m,
                    2);

            snapshot.DiskPercentUsed =
                Math.Round(
                    (1m -
                     ((decimal)drive.AvailableFreeSpace
                      / drive.TotalSize))
                     * 100m,
                    2);
        }
        catch
        {
            // Ignore disk collection failures and continue
            // collecting remaining metrics.
        }

        return Task.CompletedTask;
    }
}