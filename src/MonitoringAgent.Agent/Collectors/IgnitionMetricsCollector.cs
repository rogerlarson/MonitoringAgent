// ============================================================================
// Project: MonitoringAgent.Agent
// File: IgnitionMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects Ignition-specific metrics from the local machine.
//
//      Retrieves Ignition service status, process information, resource
//      utilization, version details, and runtime statistics used for
//      monitoring, alerting, and health reporting.
// ============================================================================

// TODO:
// Process.StartTime, TotalProcessorTime,
// and MainModule access are denied when
// the agent runs as a normal user.
// Consider retrieving uptime, CPU, and
// Java version from the Ignition Gateway API.

using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects Ignition-specific metrics.
/// </summary>
public sealed class IgnitionMetricsCollector
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly AgentSettings _settings;

    // =====================================================================
    // CPU Sampling State
    // =====================================================================

    private TimeSpan _lastTotalProcessorTime;
    private DateTime _lastCpuSampleUtc;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    public IgnitionMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings =
            settings.Value;

        _lastCpuSampleUtc =
            DateTime.UtcNow;
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates Ignition metrics on the supplied snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// Snapshot being populated.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    public Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        snapshot.IgnitionServiceRunning =
            IsIgnitionServiceRunning();

        var process =
            GetIgnitionProcess();

        snapshot.IgnitionProcessRunning =
            process != null;

        snapshot.IgnitionProcessId =
            GetIgnitionProcessId(
                process);

        snapshot.IgnitionMemoryMb =
            GetIgnitionMemoryMb(
                process);

        snapshot.IgnitionThreadCount =
            GetIgnitionThreadCount(
                process);

        snapshot.IgnitionHandleCount =
            GetIgnitionHandleCount(
                process);

        snapshot.IgnitionUptimeMinutes =
            GetIgnitionUptimeMinutes(
                process);

        snapshot.IgnitionCpuPercent =
            GetIgnitionCpuPercent(
                process);

        snapshot.IgnitionProcessName =
            process?.ProcessName
            ?? string.Empty;

        snapshot.IgnitionVersion =
            GetIgnitionVersion();

        snapshot.JavaVersion =
            GetJavaVersion(
                process);

        return Task.CompletedTask;
    }

    // =====================================================================
    // Service Detection
    // =====================================================================

    /// <summary>
    /// Determines whether the Ignition Windows service is running.
    /// </summary>
    /// <returns>
    /// True if the service is running; otherwise false.
    /// </returns>
    private bool IsIgnitionServiceRunning()
    {
        try
        {
            using var service =
                new ServiceController(
                    _settings.IgnitionServiceName);

            return service.Status ==
                   ServiceControllerStatus.Running;
        }
        catch
        {
            return false;
        }
    }

    // =====================================================================
    // Process Detection
    // =====================================================================

    /// <summary>
    /// Returns the Ignition JVM process.
    /// </summary>
    /// <returns>
    /// Ignition Java process if found; otherwise null.
    /// </returns>
    private Process? GetIgnitionProcess()
    {
        return Process
            .GetProcessesByName("java")
            .OrderByDescending(
                process => process.WorkingSet64)
            .FirstOrDefault();
    }

    /// <summary>
    /// Returns the Ignition process identifier.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Process identifier or zero if unavailable.
    /// </returns>
    private static int GetIgnitionProcessId(
        Process? process)
    {
        return process?.Id ?? 0;
    }

    // =====================================================================
    // Resource Utilization
    // =====================================================================

    /// <summary>
    /// Returns Ignition memory usage in megabytes.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Memory usage in MB.
    /// </returns>
    private static long GetIgnitionMemoryMb(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        return process.WorkingSet64
               / 1024
               / 1024;
    }

    /// <summary>
    /// Returns the Ignition thread count.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Thread count.
    /// </returns>
    private static int GetIgnitionThreadCount(
        Process? process)
    {
        return process?.Threads.Count ?? 0;
    }

    /// <summary>
    /// Returns the Ignition handle count.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Handle count.
    /// </returns>
    private static int GetIgnitionHandleCount(
        Process? process)
    {
        return process?.HandleCount ?? 0;
    }

    /// <summary>
    /// Calculates Ignition CPU utilization percentage.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// CPU utilization percentage.
    /// </returns>
    private decimal GetIgnitionCpuPercent(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        try
        {
            process.Refresh();

            var currentCpu =
                process.TotalProcessorTime;

            var currentTime =
                DateTime.UtcNow;

            if (_lastTotalProcessorTime == TimeSpan.Zero)
            {
                _lastTotalProcessorTime =
                    currentCpu;

                _lastCpuSampleUtc =
                    currentTime;

                return 0;
            }

            var previousCpu =
                _lastTotalProcessorTime;

            var previousTime =
                _lastCpuSampleUtc;

            var cpuUsedMs =
                (currentCpu -
                 previousCpu)
                .TotalMilliseconds;

            var elapsedMs =
                (currentTime -
                 previousTime)
                .TotalMilliseconds;

            // Update baseline for the next sample.
            _lastTotalProcessorTime =
                currentCpu;

            _lastCpuSampleUtc =
                currentTime;

            if (elapsedMs <= 0)
            {
                return 0;
            }

            var cpuPercent =
                cpuUsedMs
                / elapsedMs
                / Environment.ProcessorCount
                * 100.0;

            return Math.Round(
                (decimal)cpuPercent,
                2);
        }
        catch
        {
            return 0;
        }
    }

    // =====================================================================
    // Runtime Information
    // =====================================================================

    /// <summary>
    /// Returns Ignition uptime in minutes.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Uptime in minutes.
    /// </returns>
    private static long GetIgnitionUptimeMinutes(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        try
        {
            return (long)(
                DateTime.Now -
                process.StartTime)
                .TotalMinutes;
        }
        catch
        {
            return 0;
        }
    }

    // =====================================================================
    // Version Information
    // =====================================================================

    /// <summary>
    /// Returns the Java runtime version used by the Ignition process.
    /// </summary>
    /// <param name="process">
    /// Ignition process.
    /// </param>
    /// <returns>
    /// Java version string.
    /// </returns>
    private static string GetJavaVersion(
        Process? process)
    {
        try
        {
            return process == null
                ? string.Empty
                : FileVersionInfo
                    .GetVersionInfo(
                        process.MainModule!.FileName)
                    .ProductVersion
                    ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns the installed Ignition version.
    /// </summary>
    /// <returns>
    /// Ignition version string.
    /// </returns>
    private string GetIgnitionVersion()
    {
        try
        {
            var path =
                Path.Combine(
                    _settings.IgnitionInstallPath,
                    "lib",
                    "install-info.txt");

            // Verify the install information file exists.
            if (!File.Exists(path))
            {
                return string.Empty;
            }

            // Extract the gateway.version entry.
            foreach (var line in File.ReadAllLines(path))
            {
                if (line.StartsWith(
                    "gateway.version=",
                    StringComparison.OrdinalIgnoreCase))
                {
                    return line.Replace(
                        "gateway.version=",
                        string.Empty);
                }
            }
        }
        catch
        {
        }

        return string.Empty;
    }
}