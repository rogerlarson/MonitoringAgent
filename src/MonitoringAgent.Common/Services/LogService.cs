using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Common.Services;

/// <summary>
/// ============================================================================
/// Log Service
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Centralized logging service for the monitoring platform.
///
/// Responsibilities:
/// - Write application log entries
/// - Categorize log messages
/// - Create daily log files
/// - Prevent logging failures from impacting monitoring
///
/// Log Categories:
/// - ALERT
/// - EMAIL
/// - HEARTBEAT
/// - API
/// - ENGINE
///
/// Log Format:
///
///     2026-06-07 14:25:11Z INFO [ALERT] CPU threshold exceeded
///
/// Log Files:
///
///     log-YYYY-MM-DD.log
///
/// Notes:
/// Logging failures are intentionally suppressed to prevent
/// monitoring disruptions caused by filesystem or permission
/// issues.
///
/// ============================================================================
/// </summary>
public sealed class LogService
    : ILogService
{
    // -------------------------------------------------------------------------
    // Dependencies
    // -------------------------------------------------------------------------

    private readonly string _logDirectory;
    private readonly LogSettings _settings;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    public LogService(
        IOptions<LogSettings> settings)
    {
        _settings =
            settings.Value;

        var configuredPath =
            _settings.LogDirectory;

        //
        // Support both absolute and
        // application-relative paths.
        //
        _logDirectory =
            Path.IsPathRooted(
                configuredPath)
                ? configuredPath
                : Path.Combine(
                    AppContext.BaseDirectory,
                    configuredPath);

        Directory.CreateDirectory(
            _logDirectory);
    }

    // -------------------------------------------------------------------------
    // Core Logging
    // -------------------------------------------------------------------------

    public async Task Log(
        string category,
        LogLevel level,
        string message)
    {
        try
        {
            //
            // Single daily log file.
            //
            var fileName =
                $"log-{DateTime.UtcNow:yyyy-MM-dd}.log";

            var path =
                Path.Combine(
                    _logDirectory,
                    fileName);

            var severity =
                level
                    .ToString()
                    .ToUpperInvariant();

            var displayCategory =
                category
                    .ToUpperInvariant();

            var line =
                $"{DateTime.UtcNow:u} " +
                $"{severity,-8} " +
                $"[{displayCategory}] " +
                $"{message}" +
                Environment.NewLine;

            await File.AppendAllTextAsync(
                path,
                line);
        }
        catch
        {
            //
            // Logging must never crash
            // the monitoring platform.
            //
        }
    }

    // -------------------------------------------------------------------------
    // Alert Logging
    // -------------------------------------------------------------------------

    public Task LogAlert(
        string message)
    {
        if (!_settings.EnableAlertLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "ALERT",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // Email Logging
    // -------------------------------------------------------------------------

    public Task LogEmail(
        string message)
    {
        if (!_settings.EnableEmailLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "EMAIL",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // Heartbeat Logging
    // -------------------------------------------------------------------------

    public Task LogHeartbeat(
        string message)
    {
        if (!_settings.EnableHeartbeatLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "HEARTBEAT",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // Agent Logging
    // -------------------------------------------------------------------------

    public Task LogAgent(
        string message)
    {
        if (!_settings.EnableMaintenanceLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "AGENT",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // API Logging
    // -------------------------------------------------------------------------

    public Task LogApi(
        string message)
    {
        if (!_settings.EnableApiLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "API",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // Engine Logging
    // -------------------------------------------------------------------------

    public Task LogMaintenance(
        string message)
    {
        if (!_settings.EnableMaintenanceLogging)
        {
            return Task.CompletedTask;
        }

        return Log(
            "ENGINE",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // System Logging
    // -------------------------------------------------------------------------

    public Task LogSystem(
        string message)
    {
        return Log(
            "SYSTEM",
            LogLevel.Info,
            message);
    }

    // -------------------------------------------------------------------------
    // Warning Logging
    // -------------------------------------------------------------------------

    public Task LogWarning(
        string category,
        string message)
    {
        return Log(
            category,
            LogLevel.Warning,
            message);
    }

    // -------------------------------------------------------------------------
    // Error Logging
    // -------------------------------------------------------------------------

    public Task LogError(
        string category,
        Exception exception)
    {
        return Log(
            category,
            LogLevel.Error,
            exception.ToString());
    }
}