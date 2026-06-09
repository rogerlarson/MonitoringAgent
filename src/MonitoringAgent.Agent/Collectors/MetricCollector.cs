// ============================================================================
// Project: MonitoringAgent.Agent
// File: MetricCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Coordinates health metric collection from all registered metric
//      collectors.
//
//      Aggregates system, CPU, memory, disk, network, Ignition, gateway,
//      and host information into a single health snapshot that can be
//      published to the monitoring API.
//
//      Individual collector failures are isolated to prevent a single
//      failing collector from preventing snapshot publication.
// ============================================================================

using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Coordinates collection of health metrics from all registered collectors.
/// </summary>
public sealed class MetricCollector
    : IMetricCollector
{
    // =====================================================================
    // Collector Dependencies
    // =====================================================================

    private readonly SystemMetricsCollector _systemMetrics;
    private readonly CpuMemoryMetricsCollector _cpuMemoryMetrics;
    private readonly IgnitionMetricsCollector _ignitionMetrics;
    private readonly GatewayMetricsCollector _gatewayMetrics;
    private readonly DiskMetricsCollector _diskMetrics;
    private readonly DiskPerformanceMetricsCollector _diskPerformanceMetrics;
    private readonly NetworkMetricsCollector _networkMetrics;
    private readonly HostInformationCollector _hostInformation;
    private readonly ILogService _logService;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the metric collector.
    /// </summary>
    public MetricCollector(
        SystemMetricsCollector systemMetrics,
        CpuMemoryMetricsCollector cpuMemoryMetrics,
        IgnitionMetricsCollector ignitionMetrics,
        GatewayMetricsCollector gatewayMetrics,
        DiskMetricsCollector diskMetrics,
        DiskPerformanceMetricsCollector diskPerformanceMetrics,
        NetworkMetricsCollector networkMetrics,
        HostInformationCollector hostInformation,
        ILogService logService)
    {
        _systemMetrics =
            systemMetrics;

        _cpuMemoryMetrics =
            cpuMemoryMetrics;

        _ignitionMetrics =
            ignitionMetrics;

        _gatewayMetrics =
            gatewayMetrics;

        _diskMetrics =
            diskMetrics;

        _diskPerformanceMetrics =
            diskPerformanceMetrics;

        _networkMetrics =
            networkMetrics;

        _hostInformation =
            hostInformation;

        _logService =
            logService;
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Collects all available health metrics and returns a completed
    /// health snapshot.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// Fully populated health snapshot.
    /// </returns>
    public async Task<HealthSnapshot> CollectAsync(
        CancellationToken cancellationToken)
    {
        var snapshot =
            new HealthSnapshot
            {
                ServerName =
                    Environment.MachineName,

                SnapshotUtc =
                    DateTime.UtcNow
            };

        // =============================================================
        // System Metrics
        // =============================================================

        await SafePopulate(
            nameof(SystemMetricsCollector),
            () => _systemMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        await SafePopulate(
            nameof(CpuMemoryMetricsCollector),
            () => _cpuMemoryMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        // =============================================================
        // Ignition Metrics
        // =============================================================

        await SafePopulate(
            nameof(IgnitionMetricsCollector),
            () => _ignitionMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        await SafePopulate(
            nameof(GatewayMetricsCollector),
            () => _gatewayMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        // =============================================================
        // Storage Metrics
        // =============================================================

        await SafePopulate(
            nameof(DiskMetricsCollector),
            () => _diskMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        await SafePopulate(
            nameof(DiskPerformanceMetricsCollector),
            () => _diskPerformanceMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        // =============================================================
        // Network Metrics
        // =============================================================

        await SafePopulate(
            nameof(NetworkMetricsCollector),
            () => _networkMetrics.PopulateAsync(
                snapshot,
                cancellationToken));

        // =============================================================
        // Host Information
        // =============================================================

        await SafePopulate(
            nameof(HostInformationCollector),
            () => _hostInformation.PopulateAsync(
                snapshot,
                cancellationToken));

        return snapshot;
    }

    // =====================================================================
    // Collector Error Isolation
    // =====================================================================

    /// <summary>
    /// Executes a collector and prevents failures from impacting
    /// the remainder of snapshot collection.
    /// </summary>
    /// <param name="collectorName">
    /// Collector name.
    /// </param>
    /// <param name="collector">
    /// Collector execution delegate.
    /// </param>
    private async Task SafePopulate(
        string collectorName,
        Func<Task> collector)
    {
        try
        {
            await collector();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            await _logService.LogError(
                "AGENT",
                ex);

            await _logService.LogAgent(
                $"{collectorName} collection failed: {ex.Message}");
        }
    }
}