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
- EngineSettings.LogCleanupIntervalHours

Execution Frequency:
Configured through EngineSettings.

Default behavior is to execute periodically and remove
log files older than the configured retention period.

Notes:
This worker helps prevent uncontrolled log growth and
reduces disk consumption on MonitoringAgent hosts.

All cleanup activity is recorded through the logging
service for auditing and troubleshooting purposes.

===============================================================================
*/

using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Engine.Configuration;

namespace MonitoringAgent.Engine.Workers;

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
    private readonly EngineSettings _engineSettings;
    private readonly ILogService _logService;

    public LogCleanupWorker(
        IOptions<LogSettings> settings,
        IOptions<EngineSettings> engineOptions,
        ILogService logService)
    {
        _settings =
            settings.Value;

        _engineSettings =
            engineOptions.Value;

        _logService =
            logService;
    }

    /// <summary>
    /// Main worker execution loop.
    ///
    /// Process:
    /// 1. Scan the log directory
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
                // -------------------------------------------------------------
                // Locate Log Directory
                // -------------------------------------------------------------

                var logDirectory =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Logs");

                // -------------------------------------------------------------
                // Cleanup Expired Log Files
                // -------------------------------------------------------------

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

                        if (
                            info.LastWriteTimeUtc <
                            DateTime.UtcNow.AddDays(
                                -_settings.RetentionDays))
                        {
                            File.Delete(
                                file);

                            await _logService
                                .LogMaintenance(
                                    $"Deleted old log file: {info.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // -------------------------------------------------------------
                // Error Handling
                // -------------------------------------------------------------
                //
                // Cleanup failures should not terminate
                // the worker. Record the failure and
                // continue execution.
                //

                await _logService
                    .LogMaintenance(
                        $"Log cleanup error: {ex}");
            }

            // -------------------------------------------------------------
            // Scheduled Delay
            // -------------------------------------------------------------

            await Task.Delay(
                TimeSpan.FromHours(
                    _engineSettings
                        .LogCleanupIntervalHours),
                stoppingToken);
        }
    }
}