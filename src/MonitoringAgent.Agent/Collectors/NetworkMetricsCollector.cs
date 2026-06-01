using System.Diagnostics;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Infrastructure;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects network metrics.
/// </summary>
public sealed class NetworkMetricsCollector
    : IDisposable
{
    private readonly NetworkInterface? _networkInterface;
    private readonly PerformanceCounter _bytesReceivedCounter;
    private readonly PerformanceCounter _bytesSentCounter;
    private readonly PerformanceCounter _tcpRetransCounter;
    private readonly AgentSettings _settings;

    public NetworkMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings = settings.Value;
        
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

        _bytesReceivedCounter.NextValue();
        _bytesSentCounter.NextValue();
        _tcpRetransCounter.NextValue();
    }

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
            nic?.Description ??
            _settings.NetworkInterfaceName;

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

    public void Dispose()
    {
        _bytesReceivedCounter.Dispose();
        _bytesSentCounter.Dispose();
        _tcpRetransCounter.Dispose();
    }
}