// ============================================================================
// Project : MonitoringAgent.Agent
// File    : WindowsPerformanceCounterFactory.cs
//
// Purpose
// -------
// Creates Windows performance counters used throughout the monitoring agent.
//
// ============================================================================

using System.Diagnostics;
using System.Net.NetworkInformation;

namespace MonitoringAgent.Agent.Infrastructure;

/// <summary>
/// Creates Windows performance counters.
/// </summary>
public static class WindowsPerformanceCounterFactory
{
    /// <summary>
    /// Creates the total CPU utilization counter.
    /// </summary>
    public static PerformanceCounter CreateCpuCounter()
    {
        return new PerformanceCounter(
            "Processor",
            "% Processor Time",
            "_Total");
    }

    /// <summary>
    /// Creates the available memory counter.
    /// </summary>
    public static PerformanceCounter CreateAvailableMemoryCounter()
    {
        return new PerformanceCounter(
            "Memory",
            "Available MBytes");
    }

    /// <summary>
    /// Creates the page faults counter.
    /// </summary>
    public static PerformanceCounter CreatePageFaultCounter()
    {
        return new PerformanceCounter(
            "Memory",
            "Page Faults/sec");
    }

    /// <summary>
    /// Creates the context switches counter.
    /// </summary>
    public static PerformanceCounter CreateContextSwitchCounter()
    {
        return new PerformanceCounter(
            "System",
            "Context Switches/sec");
    }

    /// <summary>
    /// Creates the disk reads per second counter.
    /// </summary>
    public static PerformanceCounter CreateDiskReadCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Disk Reads/sec",
            "_Total");
    }

    /// <summary>
    /// Creates the disk writes per second counter.
    /// </summary>
    public static PerformanceCounter CreateDiskWriteCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Disk Writes/sec",
            "_Total");
    }

    /// <summary>
    /// Creates the current disk queue length counter.
    /// </summary>
    public static PerformanceCounter CreateDiskQueueCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Current Disk Queue Length",
            "_Total");
    }

    /// <summary>
    /// Creates the network bytes received counter.
    /// </summary>
    public static PerformanceCounter CreateNetworkBytesReceivedCounter(
        string interfaceName)
    {
        return new PerformanceCounter(
            "Network Interface",
            "Bytes Received/sec",
            interfaceName);
    }

    /// <summary>
    /// Creates the network bytes sent counter.
    /// </summary>
    public static PerformanceCounter CreateNetworkBytesSentCounter(
        string interfaceName)
    {
        return new PerformanceCounter(
            "Network Interface",
            "Bytes Sent/sec",
            interfaceName);
    }

    /// <summary>
    /// Creates the TCP retransmissions counter.
    /// </summary>
    public static PerformanceCounter CreateTcpRetransmissionsCounter()
    {
        return new PerformanceCounter(
            "TCPv4",
            "Segments Retransmitted/sec");
    }

    /// <summary>
    /// Creates the disk read latency counter
    /// </summary>
    public static PerformanceCounter CreateDiskReadLatencyCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk sec/Read",
            "_Total");
    }

    /// <summary>
    /// Creates the disk write latency counter
    /// </summary>
    public static PerformanceCounter CreateDiskWriteLatencyCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk sec/Write",
            "_Total");
    }
    
    /// <summary>
    /// Creates the avg disk queue counter
    /// </summary>
    public static PerformanceCounter CreateAvgDiskQueueCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk Queue Length",
            "_Total");
    }

    /// <summary>
    /// Gets the primary network interface (helper class)
    /// </summary>
    private static string GetPrimaryNetworkInterface()
    {
        return NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(n =>
                n.OperationalStatus ==
                OperationalStatus.Up)
            .Where(n =>
                n.NetworkInterfaceType !=
                NetworkInterfaceType.Loopback)
            .OrderByDescending(n =>
                n.Speed)
            .First()
            .Description;
    }
}