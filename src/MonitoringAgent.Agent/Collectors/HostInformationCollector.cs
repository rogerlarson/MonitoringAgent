// ============================================================================
// Project: MonitoringAgent.Agent
// File: HostInformationCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects static host information from the local machine.
//
//      Host information includes operating system details, processor
//      information, and installed memory capacity. These values typically
//      change infrequently and provide additional context for monitoring
//      and inventory reporting.
// ============================================================================

using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects static host information from the local machine.
/// </summary>
public sealed class HostInformationCollector
{
    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates host information on the supplied snapshot.
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

    // =====================================================================
    // Host Information Helpers
    // =====================================================================

    /// <summary>
    /// Retrieves total available system memory in megabytes.
    /// </summary>
    /// <returns>
    /// Total memory in MB, or zero if the value cannot be determined.
    /// </returns>
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