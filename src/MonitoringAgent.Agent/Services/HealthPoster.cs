// ============================================================================
// Project: MonitoringAgent.Agent
// File: HealthPoster.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Publishes health snapshots collected by the monitoring agent.
//
//      Responsible for transmitting snapshot data to the central monitoring
//      API using HTTP requests and optional API key authentication.
// ============================================================================

using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Common.Models;
using System.Net.Http.Json;

namespace MonitoringAgent.Agent.Services;

/// <summary>
/// Publishes health snapshots to the monitoring API.
/// </summary>
public sealed class HealthPoster
    : IHealthPoster
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly ILogger<HealthPoster> _logger;
    private readonly HttpClient _httpClient;
    private readonly AgentSettings _settings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes the health snapshot publisher.
    /// </summary>
    /// <param name="logger">
    /// Logging service.
    /// </param>
    /// <param name="httpClientFactory">
    /// Factory used to create HTTP clients.
    /// </param>
    /// <param name="settings">
    /// Agent configuration settings.
    /// </param>
    public HealthPoster(
        ILogger<HealthPoster> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AgentSettings> settings)
    {
        _logger =
            logger;

        _httpClient =
            httpClientFactory.CreateClient();

        _settings =
            settings.Value;

        _logger.LogInformation(
            "API key configured: {Configured}",
            !string.IsNullOrWhiteSpace(
                _settings.ApiKey));
    }

    // =====================================================================
    // Snapshot Publishing
    // =====================================================================

    /// <summary>
    /// Publishes a health snapshot to the monitoring API.
    /// </summary>
    /// <param name="snapshot">
    /// Snapshot to publish.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token for the operation.
    /// </param>
    public async Task PublishAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create HTTP request containing the snapshot payload.
            var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    _settings.CollectorUrl)
                {
                    Content =
                        JsonContent.Create(
                            snapshot)
                };

            // Add API key header when configured.
            if (!string.IsNullOrWhiteSpace(
                    _settings.ApiKey))
            {
                request.Headers.Add(
                    "X-API-Key",
                    _settings.ApiKey);
            }

            // Submit snapshot to the monitoring API.
            var response =
                await _httpClient.SendAsync(
                    request,
                    cancellationToken);

            // Throw an exception for non-success responses.
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to publish snapshot.");
        }
    }
}