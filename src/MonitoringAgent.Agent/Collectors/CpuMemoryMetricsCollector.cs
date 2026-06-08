// ============================================================================
// Project: MonitoringAgent.Agent
// File: CpuMemoryMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects CPU and memory metrics from the local machine.
//
//      Uses Windows performance counters and WMI to gather processor,
//      memory, paging, and context switching statistics for inclusion in
//      health snapshots.
// ============================================================================

using System.Diagnostics;
using System.Management;
using MonitoringAgent.Agent.Infrastructure;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects CPU and memory metrics from the local machine.
/// </summary>
public sealed class CpuMemoryMetricsCollector
    : IDisposable
{
    // =====================================================================
    // Performance Counters
    // =====================================================================

    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _availableMemoryCounter;
    private readonly PerformanceCounter _pageFaultCounter;
    private readonly PerformanceCounter _contextSwitchCounter;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    public CpuMemoryMetricsCollector()
    {
        _cpuCounter =
            WindowsPerformanceCounterFactory
                .CreateCpuCounter();

        _availableMemoryCounter =
            WindowsPerformanceCounterFactory
                .CreateAvailableMemoryCounter();

        _pageFaultCounter =
            WindowsPerformanceCounterFactory
                .CreatePageFaultCounter();

        _contextSwitchCounter =
            WindowsPerformanceCounterFactory
                .CreateContextSwitchCounter();

        // Prime performance counters. Many Windows counters require an
        // initial sample before returning meaningful values.

        _cpuCounter.NextValue();

        _availableMemoryCounter.NextValue();

        _pageFaultCounter.NextValue();

        _contextSwitchCounter.NextValue();
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates CPU and memory metrics on the supplied snapshot.
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
        var cpuPercent =
            Math.Round(
                Convert.ToDecimal(
                    _cpuCounter.NextValue()),
                2);

        var availableMemoryMb =
            Convert.ToInt64(
                _availableMemoryCounter.NextValue());

        var pageFaultsPerSec =
            Math.Round(
                Convert.ToDecimal(
                    _pageFaultCounter.NextValue()),
                2);

        var contextSwitchesPerSec =
            Math.Round(
                Convert.ToDecimal(
                    _contextSwitchCounter.NextValue()),
                2);

        var totalMemoryMb =
            GetTotalMemoryMb();

        decimal memoryPercent = 0;

        if (totalMemoryMb > 0)
        {
            memoryPercent =
                Math.Round(
                    ((decimal)(totalMemoryMb - availableMemoryMb)
                    / totalMemoryMb) * 100m,
                    2);
        }

        snapshot.CpuPercent =
            cpuPercent;

        snapshot.AvailableMemoryMb =
            availableMemoryMb;

        snapshot.MemoryPercent =
            memoryPercent;

        snapshot.PageFaultsPerSec =
            pageFaultsPerSec;

        snapshot.ContextSwitchesPerSec =
            contextSwitchesPerSec;

        return Task.CompletedTask;
    }

    // =====================================================================
    // Memory Information
    // =====================================================================

    /// <summary>
    /// Retrieves total installed physical memory in megabytes.
    /// </summary>
    /// <returns>
    /// Total physical memory in MB.
    /// </returns>
    private static long GetTotalMemoryMb()
    {
        using var searcher =
            new ManagementObjectSearcher(
                "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");

        foreach (ManagementObject obj in searcher.Get())
        {
            return Convert.ToInt64(
                obj["TotalPhysicalMemory"])
                / 1024
                / 1024;
        }

        return 0;
    }

    // =====================================================================
    // Cleanup
    // =====================================================================

    /// <inheritdoc />
    public void Dispose()
    {
        _cpuCounter.Dispose();
        _availableMemoryCounter.Dispose();
        _pageFaultCounter.Dispose();
        _contextSwitchCounter.Dispose();
    }
}