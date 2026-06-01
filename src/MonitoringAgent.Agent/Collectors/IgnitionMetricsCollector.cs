// ============================================================================
// Project : MonitoringAgent.Agent
// File    : IgnitionMetricsCollector.cs
//
// Purpose
// -------
// Collects Ignition-specific metrics.
//
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
    private readonly AgentSettings _settings;
    private TimeSpan _lastTotalProcessorTime;
    private DateTime _lastCpuSampleUtc;

    public IgnitionMetricsCollector(
        IOptions<AgentSettings> settings)
    {
        _settings = settings.Value;
        _lastCpuSampleUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Populates Ignition metrics on the supplied snapshot.
    /// </summary>
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
            GetIgnitionProcessId(process);

        snapshot.IgnitionMemoryMb =
            GetIgnitionMemoryMb(process);

        snapshot.IgnitionThreadCount =
            GetIgnitionThreadCount(process);

        snapshot.IgnitionHandleCount =
            GetIgnitionHandleCount(process);

        snapshot.IgnitionUptimeMinutes =
            GetIgnitionUptimeMinutes(process);

        snapshot.IgnitionCpuPercent =
            GetIgnitionCpuPercent(process);

        snapshot.IgnitionProcessName =
            process?.ProcessName ?? string.Empty;

        snapshot.IgnitionVersion =
            GetIgnitionVersion();

        snapshot.JavaVersion =
            GetJavaVersion(process);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Determines whether the Ignition Windows service is running.
    /// </summary>
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

    /// <summary>
    /// Returns the Ignition JVM process.
    /// </summary>
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
    private static int GetIgnitionProcessId(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        return process.Id;
    }

    /// <summary>
    /// Returns Ignition memory usage in megabytes.
    /// </summary>
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
    private static int GetIgnitionThreadCount(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        return process.Threads.Count;
    }

    /// <summary>
    /// Returns the Ignition handle count.
    /// </summary>
    private static int GetIgnitionHandleCount(
        Process? process)
    {
        if (process == null)
        {
            return 0;
        }

        return process.HandleCount;
    }

    /// <summary>
    /// Returns Ignition uptime in minutes.
    /// </summary>
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
        catch (Exception ex)
        {
            Console.WriteLine(
                $"UPTIME ERROR: {ex}");

            return 0;
        }
    }

    /// <summary>
    /// Returns Ignition CPU %...
    /// </summary>
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

            // IMPORTANT:
            // Update baseline for next sample.
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
        catch (Exception ex)
        {
            Console.WriteLine(
                $"CPU ERROR: {ex}");

            return 0;
        }
    }

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

    private string GetIgnitionVersion()
    {
        try
        {
            var path =
                Path.Combine(
                    _settings.IgnitionInstallPath,
                    "lib",
                    "install-info.txt");
            // Check if install-info.txt file exists in /lib/
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            // Loop over all lines in install-info.txt to get the gateway.version=X.Y.Z
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