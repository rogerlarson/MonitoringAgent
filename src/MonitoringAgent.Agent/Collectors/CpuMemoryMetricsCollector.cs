// ============================================================================
// Project : MonitoringAgent.Agent
// File    : CpuMemoryMetricsCollector.cs
//
// Purpose
// -------
// Collects CPU and memory metrics from the local machine.
//
// ============================================================================

using System.Diagnostics;
using System.Management;
using MonitoringAgent.Agent.Infrastructure;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects CPU and memory metrics.
/// </summary>
public sealed class CpuMemoryMetricsCollector : IDisposable
{
    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _availableMemoryCounter;
    private readonly PerformanceCounter _pageFaultCounter;
    private readonly PerformanceCounter _contextSwitchCounter;

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

        // Prime performance counters.
        // Many Windows counters require an initial sample
        // before returning meaningful values.

        _cpuCounter.NextValue();

        _availableMemoryCounter.NextValue();

        _pageFaultCounter.NextValue();

        _contextSwitchCounter.NextValue();
    }

    /// <summary>
    /// Populates CPU and memory metrics.
    /// </summary>
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

    /// <summary>
    /// Returns total physical memory in MB.
    /// </summary>
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

/// <inheritdoc />
public void Dispose()
    {
        _cpuCounter.Dispose();
        _availableMemoryCounter.Dispose();
        _pageFaultCounter.Dispose();
        _contextSwitchCounter.Dispose();
    }
}