// ============================================================================
// Project: MonitoringAgent.Agent
// File: WindowsPerformanceCounterFactory.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Creates Windows performance counters used throughout the monitoring
//      agent.
//
//      Centralizes performance counter creation to provide consistent
//      configuration and simplify access to Windows performance metrics
//      collected by monitoring services.
// ============================================================================

using System.Diagnostics;
using System.Net.NetworkInformation;

namespace MonitoringAgent.Agent.Infrastructure;

/// <summary>
/// Creates Windows performance counters used by monitoring collectors.
/// </summary>
public static class WindowsPerformanceCounterFactory
{
    // =====================================================================
    // System Counters
    // =====================================================================

    /// <summary>
    /// Creates the total CPU utilization counter.
    /// </summary>
    /// <returns>
    /// Performance counter for total CPU utilization.
    /// </returns>
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
    /// <returns>
    /// Performance counter for available physical memory.
    /// </returns>
    public static PerformanceCounter CreateAvailableMemoryCounter()
    {
        return new PerformanceCounter(
            "Memory",
            "Available MBytes");
    }

    /// <summary>
    /// Creates the page faults counter.
    /// </summary>
    /// <returns>
    /// Performance counter for page faults per second.
    /// </returns>
    public static PerformanceCounter CreatePageFaultCounter()
    {
        return new PerformanceCounter(
            "Memory",
            "Page Faults/sec");
    }

    /// <summary>
    /// Creates the context switches counter.
    /// </summary>
    /// <returns>
    /// Performance counter for context switches per second.
    /// </returns>
    public static PerformanceCounter CreateContextSwitchCounter()
    {
        return new PerformanceCounter(
            "System",
            "Context Switches/sec");
    }

    // =====================================================================
    // Disk Counters
    // =====================================================================

    /// <summary>
    /// Creates the disk reads per second counter.
    /// </summary>
    /// <returns>
    /// Performance counter for disk read operations.
    /// </returns>
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
    /// <returns>
    /// Performance counter for disk write operations.
    /// </returns>
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
    /// <returns>
    /// Performance counter for current disk queue length.
    /// </returns>
    public static PerformanceCounter CreateDiskQueueCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Current Disk Queue Length",
            "_Total");
    }

    /// <summary>
    /// Creates the average disk read latency counter.
    /// </summary>
    /// <returns>
    /// Performance counter for disk read latency.
    /// </returns>
    public static PerformanceCounter CreateDiskReadLatencyCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk sec/Read",
            "_Total");
    }

    /// <summary>
    /// Creates the average disk write latency counter.
    /// </summary>
    /// <returns>
    /// Performance counter for disk write latency.
    /// </returns>
    public static PerformanceCounter CreateDiskWriteLatencyCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk sec/Write",
            "_Total");
    }

    /// <summary>
    /// Creates the average disk queue length counter.
    /// </summary>
    /// <returns>
    /// Performance counter for average disk queue length.
    /// </returns>
    public static PerformanceCounter CreateAvgDiskQueueCounter()
    {
        return new PerformanceCounter(
            "PhysicalDisk",
            "Avg. Disk Queue Length",
            "_Total");
    }

    // =====================================================================
    // Network Counters
    // =====================================================================

    /// <summary>
    /// Creates the network bytes received counter.
    /// </summary>
    /// <param name="interfaceName">
    /// Network interface instance name.
    /// </param>
    /// <returns>
    /// Performance counter for received network traffic.
    /// </returns>
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
    /// <param name="interfaceName">
    /// Network interface instance name.
    /// </param>
    /// <returns>
    /// Performance counter for transmitted network traffic.
    /// </returns>
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
    /// <returns>
    /// Performance counter for TCP retransmissions per second.
    /// </returns>
    public static PerformanceCounter CreateTcpRetransmissionsCounter()
    {
        return new PerformanceCounter(
            "TCPv4",
            "Segments Retransmitted/sec");
    }

    // =====================================================================
    // Network Helpers
    // =====================================================================

    /// <summary>
    /// Gets the primary active network interface.
    /// </summary>
    /// <returns>
    /// Description of the fastest active non-loopback network interface.
    /// </returns>
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