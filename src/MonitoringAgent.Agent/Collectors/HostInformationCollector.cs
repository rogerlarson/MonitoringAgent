// ============================================================================
// Project : MonitoringAgent.Agent
// File    : HostInformationCollector.cs
//
// Purpose
// -------
// Collects static host information.
//
// ============================================================================

using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects static host information.
/// </summary>
public sealed class HostInformationCollector
{
    /// <summary>
    /// Populates host information.
    /// </summary>
    public Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        snapshot.OperatingSystem =
            Environment.OSVersion.Platform
                .ToString();

        snapshot.OperatingSystemVersion =
            Environment.OSVersion.VersionString;

        snapshot.ProcessorCount =
            Environment.ProcessorCount;

        snapshot.TotalMemoryMb =
            GetTotalMemoryMb();

        return Task.CompletedTask;
    }

    private static long GetTotalMemoryMb()
    {
        try
        {
            return GC
                .GetGCMemoryInfo()
                .TotalAvailableMemoryBytes
                / 1024
                / 1024;
        }
        catch
        {
            return 0;
        }
    }
}