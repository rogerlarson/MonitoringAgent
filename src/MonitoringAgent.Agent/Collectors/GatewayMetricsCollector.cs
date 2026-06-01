// ============================================================================
// Project : MonitoringAgent.Agent
// File    : GatewayMetricsCollector.cs
//
// Purpose
// -------
// Collects Ignition Gateway availability metrics.
//
// ============================================================================

using System.Diagnostics;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects Ignition Gateway metrics.
/// </summary>
public sealed class GatewayMetricsCollector
{
    private readonly AgentSettings _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    public GatewayMetricsCollector(
        IOptions<AgentSettings> settings,
        IHttpClientFactory httpClientFactory)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Populates gateway metrics.
    /// </summary>
    public async Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            using var client =
                _httpClientFactory.CreateClient();

            client.Timeout =
                TimeSpan.FromSeconds(
                    _settings.HttpTimeoutSeconds);

            using var response =
                await client.GetAsync(
                    _settings.GatewayUrl,
                    cancellationToken);

            stopwatch.Stop();

            // TODO: Logger to console for testing...
            Console.WriteLine(
                $"Gateway response: {(int)response.StatusCode} {response.StatusCode}");

            // Write the Gateway snapshot values...
            snapshot.GatewayReachable =
                response.IsSuccessStatusCode;

            snapshot.GatewayResponseMs =
                stopwatch.ElapsedMilliseconds;

            snapshot.GatewayHttpStatusCode =
                (int)response.StatusCode;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            // TODO: Logger debug message for errors...
            Console.WriteLine(
                $"Gateway check failed: {ex}");

            snapshot.GatewayReachable = false;
            snapshot.GatewayResponseMs = 0;
            snapshot.GatewayHttpStatusCode = 500;
        }
    }
}