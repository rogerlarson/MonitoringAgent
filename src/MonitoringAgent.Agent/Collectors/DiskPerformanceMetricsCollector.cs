// ============================================================================
// Project: MonitoringAgent.Agent
// File: DiskPerformanceMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects disk performance metrics from the local machine.
//
//      Uses Windows performance counters to gather disk throughput,
//      latency, and queue statistics for inclusion in health snapshots.
//      Handles performance counter failures gracefully and logs recovery
//      events when counters become available again.
// ============================================================================

using System.Diagnostics;
using MonitoringAgent.Agent.Infrastructure;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects disk performance metrics.
/// </summary>
public sealed class DiskPerformanceMetricsCollector
    : IDisposable
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly ILogger<DiskPerformanceMetricsCollector> _logger;

    // =====================================================================
    // Performance Counters
    // =====================================================================

    private readonly PerformanceCounter _diskReadCounter;
    private readonly PerformanceCounter _diskWriteCounter;
    private readonly PerformanceCounter _diskQueueCounter;
    private readonly PerformanceCounter _diskReadLatencyCounter;
    private readonly PerformanceCounter _diskWriteLatencyCounter;
    private readonly PerformanceCounter _avgDiskQueueCounter;

    // =====================================================================
    // State
    // =====================================================================

    private readonly HashSet<string> _failedCounters =
        new();

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    /// <param name="logger">
    /// Logging service.
    /// </param>
    public DiskPerformanceMetricsCollector(
        ILogger<DiskPerformanceMetricsCollector> logger)
    {
        _logger =
            logger;

        _diskReadCounter =
            WindowsPerformanceCounterFactory
                .CreateDiskReadCounter();

        _diskWriteCounter =
            WindowsPerformanceCounterFactory
                .CreateDiskWriteCounter();

        _diskQueueCounter =
            WindowsPerformanceCounterFactory
                .CreateDiskQueueCounter();

        _diskReadLatencyCounter =
            WindowsPerformanceCounterFactory
                .CreateDiskReadLatencyCounter();

        _diskWriteLatencyCounter =
            WindowsPerformanceCounterFactory
                .CreateDiskWriteLatencyCounter();

        _avgDiskQueueCounter =
            WindowsPerformanceCounterFactory
                .CreateAvgDiskQueueCounter();

        // Prime performance counters. Many Windows counters require an
        // initial sample before returning meaningful values.

        _diskReadCounter.NextValue();
        _diskWriteCounter.NextValue();
        _diskQueueCounter.NextValue();
        _diskReadLatencyCounter.NextValue();
        _diskWriteLatencyCounter.NextValue();
        _avgDiskQueueCounter.NextValue();
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates disk performance metrics on the supplied snapshot.
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
        snapshot.DiskReadsPerSec =
            GetCounterValue(
                _diskReadCounter);

        snapshot.DiskWritesPerSec =
            GetCounterValue(
                _diskWriteCounter);

        snapshot.DiskQueueLength =
            GetCounterValue(
                _diskQueueCounter);

        snapshot.DiskReadLatencyMs =
            GetCounterValue(
                _diskReadLatencyCounter,
                1000m);

        snapshot.DiskWriteLatencyMs =
            GetCounterValue(
                _diskWriteLatencyCounter,
                1000m);

        snapshot.AvgDiskQueueLength =
            GetCounterValue(
                _avgDiskQueueCounter);

        return Task.CompletedTask;
    }

    // =====================================================================
    // Performance Counter Helpers
    // =====================================================================

    /// <summary>
    /// Retrieves the current value from a performance counter.
    /// </summary>
    /// <param name="counter">
    /// Performance counter to read.
    /// </param>
    /// <param name="multiplier">
    /// Optional multiplier applied to the returned value.
    /// </param>
    /// <returns>
    /// Rounded counter value or zero if the counter cannot be read.
    /// </returns>
    private decimal GetCounterValue(
        PerformanceCounter counter,
        decimal multiplier = 1m)
    {
        try
        {
            var key =
                $"{counter.CategoryName}\\{counter.CounterName}";

            // Log recovery when a previously failed counter becomes
            // available again.
            if (_failedCounters.Remove(key))
            {
                _logger.LogInformation(
                    "Performance counter recovered: {Counter}",
                    key);
            }

            var value =
                counter.NextValue();

            return Math.Round(
                Convert.ToDecimal(value) * multiplier,
                2);
        }
        catch (Exception ex)
        {
            var key =
                $"{counter.CategoryName}\\{counter.CounterName}";

            // Log the failure only once until the counter recovers.
            if (_failedCounters.Add(key))
            {
                _logger.LogWarning(
                    ex,
                    "Failed reading performance counter {Counter}",
                    key);
            }

            return 0;
        }
    }

    // =====================================================================
    // Cleanup
    // =====================================================================

    /// <inheritdoc />
    public void Dispose()
    {
        _diskReadCounter.Dispose();
        _diskWriteCounter.Dispose();
        _diskQueueCounter.Dispose();
        _diskReadLatencyCounter.Dispose();
        _diskWriteLatencyCounter.Dispose();
        _avgDiskQueueCounter.Dispose();
    }
}