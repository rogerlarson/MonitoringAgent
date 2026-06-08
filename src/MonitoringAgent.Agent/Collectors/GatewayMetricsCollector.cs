// ============================================================================
// Project: MonitoringAgent.Agent
// File: GatewayMetricsCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Collects Ignition Gateway availability metrics.
//
//      Verifies gateway connectivity, measures response time, and records
//      HTTP status information used for health monitoring and alerting.
// ============================================================================

using System.Diagnostics;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors;

/// <summary>
/// Collects Ignition Gateway availability metrics.
/// </summary>
public sealed class GatewayMetricsCollector
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly AgentSettings _settings;
    private readonly IHttpClientFactory _httpClientFactory;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the collector.
    /// </summary>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    /// <param name="httpClientFactory">
    /// Factory used to create HTTP clients.
    /// </param>
    public GatewayMetricsCollector(
        IOptions<AgentSettings> settings,
        IHttpClientFactory httpClientFactory)
    {
        _settings =
            settings.Value;

        _httpClientFactory =
            httpClientFactory;
    }

    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Populates gateway availability metrics on the supplied snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// Snapshot being populated.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    public async Task PopulateAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        var stopwatch =
            Stopwatch.StartNew();

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

            // Populate gateway metrics.
            snapshot.GatewayReachable =
                response.IsSuccessStatusCode;

            snapshot.GatewayResponseMs =
                stopwatch.ElapsedMilliseconds;

            snapshot.GatewayHttpStatusCode =
                (int)response.StatusCode;
        }
        catch
        {
            stopwatch.Stop();

            // Any exception is treated as an unavailable gateway.
            snapshot.GatewayReachable =
                false;

            snapshot.GatewayResponseMs =
                0;

            snapshot.GatewayHttpStatusCode =
                500;
        }
    }
}