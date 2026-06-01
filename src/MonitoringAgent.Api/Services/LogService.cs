using Microsoft.Extensions.Options;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Services.Interfaces;
using System.Runtime;

namespace MonitoringAgent.Api.Services;

public sealed class LogService
    : ILogService
{
    private readonly string _logDirectory;
    private readonly LogSettings _settings;

    public LogService(
        IOptions<LogSettings> settings)
    {
        _settings =
            settings.Value;

        var configuredPath =
            _settings.LogDirectory;

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

    public Task LogAlert(
        string message)
    {
        if (!_settings.EnableAlertLogging)
        {
            return Task.CompletedTask;
        }

        return Write(
            "alerts",
            message);
    }

    public Task LogEmail(
        string message)
    {
        if (!_settings.EnableEmailLogging)
        {
            return Task.CompletedTask;
        }

        return Write(
            "email",
            message);
    }

    public Task LogHeartbeat(
        string message)
    {
        if (!_settings.EnableHeartbeatLogging)
        {
            return Task.CompletedTask;
        }

        return Write(
            "heartbeat",
            message);
    }

    public Task LogApi(
        string message)
    {
        if (!_settings.EnableApiLogging)
        {
            return Task.CompletedTask;
        }

        return Write(
            "api",
            message);
    }

    public Task LogMaintenance(
    string message)
    {
        if (!_settings.EnableMaintenanceLogging)
        {
            return Task.CompletedTask;
        }

        return Write(
            "maintenance",
            message);
    }

    private async Task Write(
    string prefix,
    string message)
    {
        try
        {
            var fileName =
                $"{prefix}-{DateTime.UtcNow:yyyy-MM-dd}.log";

            var path =
                Path.Combine(
                    _logDirectory,
                    fileName);

            await File.AppendAllTextAsync(
                path,
                $"{DateTime.UtcNow:u} {message}{Environment.NewLine}");
        }
        catch
        {
            // Never allow logging failures
            // to crash the application.
        }
    }
}