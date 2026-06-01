using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Common.Models;
using Microsoft.Extensions.Options;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects disk space metrics.
/// </summary>
public sealed class DiskMetricsCollector
{
    private readonly AgentSettings _settings;

    public DiskMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings = settings.Value;
    }

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
            // Ignore disk collection failures.
        }

        return Task.CompletedTask;
    }
}