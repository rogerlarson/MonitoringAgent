// ============================================================================
// Project: MonitoringAgent.Agent
// File: MetricCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Coordinates health metric collection from all registered metric
//      collectors.
//
//      Aggregates system, CPU, memory, disk, network, Ignition, gateway,
//      and host information into a single health snapshot that can be
//      published to the monitoring API.
// ============================================================================

using MonitoringAgent.Agent.Collectors.Interfaces;
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

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the metric collector.
    /// </summary>
    /// <param name="systemMetrics">
    /// System metrics collector.
    /// </param>
    /// <param name="cpuMemoryMetrics">
    /// CPU and memory metrics collector.
    /// </param>
    /// <param name="ignitionMetrics">
    /// Ignition metrics collector.
    /// </param>
    /// <param name="gatewayMetrics">
    /// Gateway metrics collector.
    /// </param>
    /// <param name="diskMetrics">
    /// Disk utilization metrics collector.
    /// </param>
    /// <param name="diskPerformanceMetrics">
    /// Disk performance metrics collector.
    /// </param>
    /// <param name="networkMetrics">
    /// Network metrics collector.
    /// </param>
    /// <param name="hostInformation">
    /// Host information collector.
    /// </param>
    public MetricCollector(
        SystemMetricsCollector systemMetrics,
        CpuMemoryMetricsCollector cpuMemoryMetrics,
        IgnitionMetricsCollector ignitionMetrics,
        GatewayMetricsCollector gatewayMetrics,
        DiskMetricsCollector diskMetrics,
        DiskPerformanceMetricsCollector diskPerformanceMetrics,
        NetworkMetricsCollector networkMetrics,
        HostInformationCollector hostInformation)
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

        await _systemMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _cpuMemoryMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        // =============================================================
        // Ignition Metrics
        // =============================================================

        await _ignitionMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _gatewayMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        // =============================================================
        // Storage Metrics
        // =============================================================

        await _diskMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _diskPerformanceMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        // =============================================================
        // Network Metrics
        // =============================================================

        await _networkMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        // =============================================================
        // Host Information
        // =============================================================

        await _hostInformation.PopulateAsync(
            snapshot,
            cancellationToken);

        return snapshot;
    }
}