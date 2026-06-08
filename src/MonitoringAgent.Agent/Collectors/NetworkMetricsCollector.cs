// ============================================================================
// Project: MonitoringAgent.Agent
// File: NetworkMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects network utilization and network health metrics from the
//      configured network interface.
//
//      Uses Windows performance counters and network interface statistics
//      to gather throughput, error, discard, and retransmission metrics
//      for inclusion in health snapshots.
// ============================================================================

using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Infrastructure;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects network utilization and network health metrics.
/// </summary>
public sealed class NetworkMetricsCollector
    : IDisposable
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly AgentSettings _settings;

    // =====================================================================
    // Network Resources
    // =====================================================================

    private readonly NetworkInterface? _networkInterface;

    private readonly PerformanceCounter _bytesReceivedCounter;
    private readonly PerformanceCounter _bytesSentCounter;
    private readonly PerformanceCounter _tcpRetransCounter;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    public NetworkMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings =
            settings.Value;

        _networkInterface =
            NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(x =>
                    x.Description ==
                    _settings.NetworkInterfaceName
                    ||
                    x.Name ==
                    _settings.NetworkInterfaceName);

        _bytesReceivedCounter =
            new PerformanceCounter(
                "Network Interface",
                "Bytes Received/sec",
                _settings.NetworkInterfaceName);

        _bytesSentCounter =
            new PerformanceCounter(
                "Network Interface",
                "Bytes Sent/sec",
                _settings.NetworkInterfaceName);

        _tcpRetransCounter =
            WindowsPerformanceCounterFactory
                .CreateTcpRetransmissionsCounter();

        // Prime performance counters. Many Windows counters require an
        // initial sample before returning meaningful values.

        _bytesReceivedCounter.NextValue();

        _bytesSentCounter.NextValue();

        _tcpRetransCounter.NextValue();
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates network metrics on the supplied snapshot.
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
        snapshot.NetworkBytesReceivedPerSec =
            Math.Round(
                Convert.ToDecimal(
                    _bytesReceivedCounter.NextValue()),
                2);

        snapshot.NetworkBytesSentPerSec =
            Math.Round(
                Convert.ToDecimal(
                    _bytesSentCounter.NextValue()),
                2);

        snapshot.TcpRetransmissionsPerSec =
            Math.Round(
                Convert.ToDecimal(
                    _tcpRetransCounter.NextValue()),
                2);

        var nic =
            NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(x =>
                    x.Name ==
                    _settings.NetworkInterfaceName);

        snapshot.PrimaryNetworkInterface =
            nic?.Description
            ?? _settings.NetworkInterfaceName;

        if (_networkInterface != null)
        {
            var stats =
                _networkInterface
                    .GetIPv4Statistics();

            snapshot.NetworkReceiveErrors =
                stats.IncomingPacketsWithErrors;

            snapshot.NetworkSendErrors =
                stats.OutgoingPacketsWithErrors;

            snapshot.NetworkReceiveDiscards =
                stats.IncomingPacketsDiscarded;

            snapshot.NetworkSendDiscards =
                stats.OutgoingPacketsDiscarded;
        }

        return Task.CompletedTask;
    }

    // =====================================================================
    // Cleanup
    // =====================================================================

    /// <inheritdoc />
    public void Dispose()
    {
        _bytesReceivedCounter.Dispose();
        _bytesSentCounter.Dispose();
        _tcpRetransCounter.Dispose();
    }
}