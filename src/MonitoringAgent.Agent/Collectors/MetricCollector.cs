using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects health metrics from the local machine.
/// </summary>
public sealed class MetricCollector : IMetricCollector
{
    private readonly SystemMetricsCollector _systemMetrics;
    private readonly IgnitionMetricsCollector _ignitionMetrics;
    private readonly GatewayMetricsCollector _gatewayMetrics;
    private readonly CpuMemoryMetricsCollector _cpuMemoryMetrics;
    private readonly DiskMetricsCollector _diskMetrics;
    private readonly DiskPerformanceMetricsCollector _diskPerformanceMetrics;
    private readonly NetworkMetricsCollector _networkMetrics;
    private readonly HostInformationCollector _hostInformation;

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
        _systemMetrics = systemMetrics;
        _cpuMemoryMetrics = cpuMemoryMetrics;
        _ignitionMetrics = ignitionMetrics;
        _gatewayMetrics = gatewayMetrics;
        _diskMetrics = diskMetrics;
        _diskPerformanceMetrics = diskPerformanceMetrics;
        _networkMetrics = networkMetrics;
        _hostInformation = hostInformation;
    }

    /// <inheritdoc />
    public async Task<HealthSnapshot> CollectAsync(
        CancellationToken cancellationToken)
    {
        var snapshot = new HealthSnapshot
        {
            ServerName = Environment.MachineName,
            SnapshotUtc = DateTime.UtcNow
        };

        await _systemMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _cpuMemoryMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _ignitionMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _gatewayMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _diskMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _diskPerformanceMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _networkMetrics.PopulateAsync(
            snapshot,
            cancellationToken);

        await _hostInformation.PopulateAsync(
            snapshot,
            cancellationToken);

        return snapshot;
    }
}