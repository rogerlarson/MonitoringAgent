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
    private readonly PerformanceCounter _diskReadCounter;
    private readonly PerformanceCounter _diskWriteCounter;
    private readonly PerformanceCounter _diskQueueCounter;
    private readonly PerformanceCounter _diskReadLatencyCounter;
    private readonly PerformanceCounter _diskWriteLatencyCounter;
    private readonly PerformanceCounter _avgDiskQueueCounter;
    private readonly ILogger<DiskPerformanceMetricsCollector> _logger;
    private readonly HashSet<string> _failedCounters = new();

    public DiskPerformanceMetricsCollector(ILogger<DiskPerformanceMetricsCollector> logger)
    {
        _logger = logger;

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

        // Prime the values...
        _diskReadCounter.NextValue();
        _diskWriteCounter.NextValue();
        _diskQueueCounter.NextValue();
        _diskReadLatencyCounter.NextValue();
        _diskWriteLatencyCounter.NextValue();
        _avgDiskQueueCounter.NextValue();
    }

    public Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        snapshot.DiskReadsPerSec =
            GetCounterValue(_diskReadCounter);

        snapshot.DiskWritesPerSec =
            GetCounterValue(_diskWriteCounter);

        snapshot.DiskQueueLength =
            GetCounterValue(_diskQueueCounter);

        snapshot.DiskReadLatencyMs =
            GetCounterValue(_diskReadLatencyCounter,1000m);

        snapshot.DiskWriteLatencyMs =
            GetCounterValue(_diskWriteLatencyCounter,1000m);

        snapshot.AvgDiskQueueLength =
            GetCounterValue(_avgDiskQueueCounter);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _diskReadCounter.Dispose();
        _diskWriteCounter.Dispose();
        _diskQueueCounter.Dispose();
        _diskReadLatencyCounter.Dispose();
        _diskWriteLatencyCounter.Dispose();
        _avgDiskQueueCounter.Dispose();
    }

    private decimal GetCounterValue(
        PerformanceCounter counter,
        decimal multiplier = 1m)
    {
        try
        {
            var key =
                $"{counter.CategoryName}\\{counter.CounterName}";
            
            // Log of performance counter recovery 
            if (_failedCounters.Remove(key))
            {
                _logger.LogInformation(
                    "Performance counter recovered: {Counter}",
                    key);
            }

            var value =
                counter.NextValue();

            // TODO: Debugging 
            Console.WriteLine(
                $"{counter.CounterName}={value}");

            return Math.Round(
                Convert.ToDecimal(value) * multiplier,
                2);
        }
        catch (Exception ex)
        {
            var key =
                $"{counter.CategoryName}\\{counter.CounterName}";

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
}