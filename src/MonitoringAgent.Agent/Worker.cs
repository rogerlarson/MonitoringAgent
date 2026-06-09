// ============================================================================
// Project: MonitoringAgent.Agent
// File: Worker.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Primary background service responsible for collecting health metrics
//      from the local system and publishing snapshots to the monitoring API.
//
//      Coordinates metric collection, snapshot enrichment, and snapshot
//      delivery at configured polling intervals.
//
//      Operational logging is written through the centralized logging
//      service to assist with diagnostics and troubleshooting.
// ============================================================================

using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Common.Interfaces;
using System.Reflection;

namespace MonitoringAgent.Agent;

/// <summary>
/// Primary monitoring service responsible for collecting and publishing
/// health snapshots.
/// </summary>
public sealed class Worker
    : BackgroundService
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly ILogger<Worker> _logger;
    private readonly IMetricCollector _metricCollector;
    private readonly IHealthPoster _healthPoster;
    private readonly ILogService _logService;
    private readonly AgentSettings _settings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes the monitoring worker.
    /// </summary>
    /// <param name="logger">
    /// Microsoft logging provider.
    /// </param>
    /// <param name="metricCollector">
    /// Metric collection service.
    /// </param>
    /// <param name="healthPoster">
    /// Health snapshot publishing service.
    /// </param>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    /// <param name="logService">
    /// Centralized logging service.
    /// </param>
    public Worker(
        ILogger<Worker> logger,
        IMetricCollector metricCollector,
        IHealthPoster healthPoster,
        IOptions<AgentSettings> settings,
        ILogService logService)
    {
        _logger =
            logger;

        _metricCollector =
            metricCollector;

        _healthPoster =
            healthPoster;

        _settings =
            settings.Value;

        _logService =
            logService;
    }

    // =====================================================================
    // Worker Execution
    // =====================================================================

    /// <summary>
    /// Executes the monitoring loop until the service is stopped.
    /// </summary>
    /// <param name="stoppingToken">
    /// Cancellation token used to stop the worker.
    /// </param>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var successfulCycles =
            0;

        var agentVersion =
            Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<
                    AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion
            ?? Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version?
                .ToString()
            ?? string.Empty;

        await _logService.LogAgent(
            $"Monitoring Agent worker started. Poll Interval: {_settings.PollIntervalSeconds}s");

        await _logService.LogAgent(
            $"Machine Name: {Environment.MachineName}");

        await _logService.LogAgent(
            $"Domain Name: {Environment.UserDomainName}");

        await _logService.LogAgent(
            $"Operating System: {Environment.OSVersion}");

        await _logService.LogAgent(
            $"Collector URL: {_settings.CollectorUrl}");

        await _logService.LogAgent(
            $"API Key Configured: {!string.IsNullOrWhiteSpace(_settings.ApiKey)}");

        await _logService.LogAgent(
            $"HTTP Timeout: {_settings.HttpTimeoutSeconds}s");

        await _logService.LogAgent(
            $"Gateway URL: {_settings.GatewayUrl}");

        await _logService.LogAgent(
            $"Ignition Service: {_settings.IgnitionServiceName}");

        await _logService.LogAgent(
            $"Ignition Install Path: {_settings.IgnitionInstallPath}");

        await _logService.LogAgent(
            $"Monitored Drive: {_settings.MonitoredDrive}");

        await _logService.LogAgent(
            $"Agent Version: {agentVersion}");

        await _logService.LogAgent(
            $"Network Interface: {_settings.NetworkInterfaceName}");


        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // ---------------------------------------------------------
                // Collect Metrics
                // ---------------------------------------------------------

                var snapshot =
                    await _metricCollector
                        .CollectAsync(
                            stoppingToken);

                // ---------------------------------------------------------
                // Populate Agent Metadata
                // ---------------------------------------------------------

                snapshot.AgentVersion =
                    Assembly
                        .GetExecutingAssembly()
                        .GetCustomAttribute<
                            AssemblyInformationalVersionAttribute>()
                        ?.InformationalVersion
                    ?? Assembly
                        .GetExecutingAssembly()
                        .GetName()
                        .Version?
                        .ToString()
                    ?? string.Empty;

                snapshot.DomainName =
                    Environment.UserDomainName
                    ?? string.Empty;

                // ---------------------------------------------------------
                // Publish Snapshot
                // ---------------------------------------------------------

                await _healthPoster.PublishAsync(
                    snapshot,
                    stoppingToken);

                successfulCycles++;

                // ---------------------------------------------------------
                // Periodic Health Logging
                // ---------------------------------------------------------

                if (successfulCycles % 100 == 0)
                {
                    await _logService.LogAgent(
                        $"Successfully published {successfulCycles} snapshots.");
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Metric collection failed.");

                await _logService.LogError(
                    "AGENT",
                    ex);

                await _logService.LogAgent(
                    $"Snapshot processing failed: {ex.Message}");
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(
                        _settings.PollIntervalSeconds),
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    // =====================================================================
    // Shutdown
    // =====================================================================

    /// <summary>
    /// Records worker shutdown activity.
    /// </summary>
    /// <param name="cancellationToken">
    /// Shutdown cancellation token.
    /// </param>
    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        await _logService.LogAgent(
            "Monitoring Agent worker stopping.");

        await base.StopAsync(
            cancellationToken);

        await _logService.LogAgent(
            "Monitoring Agent worker stopped.");
    }
}