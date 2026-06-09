using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Engine.Services;

/// <summary>
/// Records application lifecycle events for the monitoring engine.
/// </summary>
public sealed class EngineLifecycleService
    : IHostedService
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogService _log;
    private readonly LogSettings _logSettings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new lifecycle tracking service.
    /// </summary>
    /// <param name="lifetime">
    /// Application lifetime event provider.
    /// </param>
    /// <param name="log">
    /// Centralized logging service.
    /// </param>
    /// <param name="logSettings">
    /// Logging configuration settings.
    /// </param>
    public EngineLifecycleService(
        IHostApplicationLifetime lifetime,
        ILogService log,
        IOptions<LogSettings> logSettings)
    {
        _lifetime =
            lifetime;

        _log =
            log;

        _logSettings =
            logSettings.Value;
    }

    // =====================================================================
    // Startup
    // =====================================================================

    /// <summary>
    /// Registers application lifecycle callbacks and
    /// records engine startup activity.
    /// </summary>
    /// <param name="cancellationToken">
    /// Startup cancellation token.
    /// </param>
    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        var logDirectory =
            Path.IsPathRooted(
                _logSettings.LogDirectory)
                ? _logSettings.LogDirectory
                : Path.Combine(
                    AppContext.BaseDirectory,
                    _logSettings.LogDirectory);

        await _log.LogMaintenance(
            "Monitoring Agent Engine starting.");

        await _log.LogMaintenance(
            $"Log Directory: {logDirectory}");

        await _log.LogMaintenance(
            $"Log Retention: {_logSettings.RetentionDays} days");

        _lifetime.ApplicationStarted.Register(
            () =>
            {
                _ = _log.LogMaintenance(
                    "Monitoring Agent Engine started.");
            });

        _lifetime.ApplicationStopping.Register(
            () =>
            {
                _ = _log.LogMaintenance(
                    "Monitoring Agent Engine stopping.");
            });

        _lifetime.ApplicationStopped.Register(
            () =>
            {
                _ = _log.LogMaintenance(
                    "Monitoring Agent Engine stopped.");
            });
    }

    // =====================================================================
    // Shutdown
    // =====================================================================

    /// <summary>
    /// Invoked when the host begins shutting down.
    /// </summary>
    /// <param name="cancellationToken">
    /// Shutdown cancellation token.
    /// </param>
    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}