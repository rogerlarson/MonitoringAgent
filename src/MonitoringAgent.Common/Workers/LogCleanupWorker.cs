/*
===============================================================================
LogCleanupWorker
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Performs automated log retention management by removing
expired log files from the MonitoringAgent log directory.

Responsibilities:
- Scan the log directory
- Identify expired log files
- Delete files exceeding retention limits
- Record maintenance activity
- Record cleanup failures

Configuration:
- LogSettings.RetentionDays
- LogSettings.CleanupIntervalHours

Execution Frequency:
Configured through LogSettings.

Default behavior is to execute periodically and remove
log files older than the configured retention period.

Notes:
This worker helps prevent uncontrolled log growth and
reduces disk consumption on MonitoringAgent hosts.

All cleanup activity is recorded through the logging
service for auditing and troubleshooting purposes.

This worker is shared by both the Monitoring Agent
and Monitoring Engine.

===============================================================================
*/

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Common.Workers;

/// <summary>
/// Background worker responsible for removing
/// expired log files according to configured
/// retention policies.
/// </summary>
public sealed class LogCleanupWorker
    : BackgroundService
{
    // -------------------------------------------------------------------------
    // Dependencies
    // -------------------------------------------------------------------------

    private readonly LogSettings _settings;
    private readonly ILogService _logService;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    public LogCleanupWorker(
        IOptions<LogSettings> settings,
        ILogService logService)
    {
        _settings =
            settings.Value;

        _logService =
            logService;
    }

    // -------------------------------------------------------------------------
    // Worker Execution
    // -------------------------------------------------------------------------

    /// <summary>
    /// Main worker execution loop.
    ///
    /// Process:
    /// 1. Resolve log directory
    /// 2. Locate expired log files
    /// 3. Delete files exceeding retention limits
    /// 4. Record maintenance activity
    /// </summary>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // ---------------------------------------------------------
                // Resolve Log Directory
                // ---------------------------------------------------------

                var configuredPath =
                    _settings.LogDirectory;

                var logDirectory =
                    Path.IsPathRooted(
                        configuredPath)
                        ? configuredPath
                        : Path.Combine(
                            AppContext.BaseDirectory,
                            configuredPath);

                var deletedFiles =
                    0;

                // ---------------------------------------------------------
                // Cleanup Expired Log Files
                // ---------------------------------------------------------

                if (Directory.Exists(
                    logDirectory))
                {
                    foreach (var file in
                        Directory.GetFiles(
                            logDirectory,
                            "*.log"))
                    {
                        var info =
                            new FileInfo(
                                file);

                        if (info.LastWriteTimeUtc <
                            DateTime.UtcNow.AddDays(
                                -_settings.RetentionDays))
                        {
                            File.Delete(
                                file);

                            deletedFiles++;

                            await _logService.LogSystem(
                                $"Deleted old log file: {info.Name}");
                        }
                    }

                    await _logService.LogSystem(
                        $"Log cleanup complete. Deleted {deletedFiles} file(s). Retention: {_settings.RetentionDays} day(s).");
                }
                else
                {
                    await _logService.LogSystem(
                        $"Log directory not found: {logDirectory}");
                }
            }
            catch (Exception ex)
            {
                await _logService.LogError(
                    "SYSTEM",
                    ex);
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromHours(
                        _settings.CleanupIntervalHours),
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}