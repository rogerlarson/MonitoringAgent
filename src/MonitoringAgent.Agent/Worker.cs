using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Services.Interfaces;
using System.Runtime;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using Microsoft.VisualBasic;
using System.Reflection;

namespace MonitoringAgent.Agent;

/// <summary>
/// Primary monitoring service.
/// </summary>
public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMetricCollector _metricCollector;
    private readonly IHealthPoster _healthPoster;
    private readonly AgentSettings _settings;

    public Worker(
        ILogger<Worker> logger,
        IMetricCollector metricCollector,
        IHealthPoster healthPoster,
        IOptions<AgentSettings> settings)
    {
        _logger = logger;
        _metricCollector = metricCollector;
        _healthPoster = healthPoster;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "MonitoringAgent started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var snapshot = await _metricCollector.CollectAsync(stoppingToken);

                // Get the Agent Version from csproj file in Agent
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

                // Get the Agent Windows Domain
                snapshot.DomainName =
                    Environment.UserDomainName ??
                    string.Empty;

                _logger.LogInformation(
                    "Collected snapshot from {ServerName} at {SnapshotUtc}",
                    snapshot.ServerName,
                    snapshot.SnapshotUtc);

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

            await Task.Delay(
                TimeSpan.FromSeconds(
                    _settings.PollIntervalSeconds),
                stoppingToken);
        }
    }
}