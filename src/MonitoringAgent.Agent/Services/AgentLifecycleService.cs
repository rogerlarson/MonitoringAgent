// ============================================================================
// Project: MonitoringAgent.Agent
// File: AgentLifecycleService.cs
// Author: Roger Larson
// Date Created: 06/09/2026
// Date Updated: 06/09/2026
// Description:
//      Hosted service responsible for recording MonitoringAgent Agent
//      application lifecycle events.
//
//      Tracks startup and shutdown events and records them through the
//      centralized logging service to assist with diagnostics,
//      troubleshooting, and operational monitoring.
//
//      Lifecycle Events:
//      - Application Starting
//      - Application Started
//      - Application Stopping
//      - Application Stopped
//
//      Startup Diagnostics:
//      - Log Directory
//      - Log Retention
//
//      This service is primarily used when the Agent is hosted as a
//      Windows Service.
// ============================================================================

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Agent.Services;

/// <summary>
/// Records application lifecycle events for the monitoring agent.
/// </summary>
public sealed class AgentLifecycleService
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
    public AgentLifecycleService(
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
    /// records agent startup activity.
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

        await _log.LogAgent(
            "Monitoring Agent starting.");

        await _log.LogAgent(
            $"Log Directory: {logDirectory}");

        await _log.LogAgent(
            $"Log Retention: {_logSettings.RetentionDays} days");

        _lifetime.ApplicationStarted.Register(
            () =>
            {
                _ = _log.LogAgent(
                    "Monitoring Agent started.");
            });

        _lifetime.ApplicationStopping.Register(
            () =>
            {
                _ = _log.LogAgent(
                    "Monitoring Agent stopping.");
            });

        _lifetime.ApplicationStopped.Register(
            () =>
            {
                _ = _log.LogAgent(
                    "Monitoring Agent stopped.");
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