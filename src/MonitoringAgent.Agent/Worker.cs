// ============================================================================
// Project: MonitoringAgent.Agent
// File: Worker.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Primary background service responsible for collecting health metrics
//      from the local system and publishing snapshots to the monitoring API.
//
//      Coordinates metric collection, snapshot enrichment, and snapshot
//      delivery at configured polling intervals.
// ============================================================================

using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services.Interfaces;
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
    private readonly AgentSettings _settings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes the monitoring worker.
    /// </summary>
    /// <param name="logger">
    /// Logging service.
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
    public Worker(
        ILogger<Worker> logger,
        IMetricCollector metricCollector,
        IHealthPoster healthPoster,
        IOptions<AgentSettings> settings)
    {
        _logger =
            logger;

        _metricCollector =
            metricCollector;

        _healthPoster =
            healthPoster;

        _settings =
            settings.Value;
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
        _logger.LogInformation(
            "MonitoringAgent started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Collect current health metrics.
                var snapshot =
                    await _metricCollector
                        .CollectAsync(
                            stoppingToken);

                // Populate agent version information.
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

                // Populate Windows domain information.
                snapshot.DomainName =
                    Environment.UserDomainName
                    ?? string.Empty;

                _logger.LogInformation(
                    "Collected snapshot from {ServerName} at {SnapshotUtc}",
                    snapshot.ServerName,
                    snapshot.SnapshotUtc);

                // Publish snapshot to the monitoring API.
                await _healthPoster.PublishAsync(
                    snapshot,
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Metric collection failed.");
            }

            // Wait until the next collection cycle.
            await Task.Delay(
                TimeSpan.FromSeconds(
                    _settings.PollIntervalSeconds),
                stoppingToken);
        }
    }
}