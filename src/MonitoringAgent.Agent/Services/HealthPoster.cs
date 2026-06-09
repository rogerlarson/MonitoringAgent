// ============================================================================
// Project: MonitoringAgent.Agent
// File: HealthPoster.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Publishes health snapshots collected by the monitoring agent.
//
//      Responsible for transmitting snapshot data to the central monitoring
//      API using HTTP requests and optional API key authentication.
//
//      All communication failures are logged and propagated to the caller
//      to ensure monitoring failures are visible and actionable.
// ============================================================================

using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Common.Interfaces;
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
    private readonly ILogService _logService;

    private bool _firstPublishSucceeded;
    private int _consecutiveFailures;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes the health snapshot publisher.
    /// </summary>
    public HealthPoster(
        ILogger<HealthPoster> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AgentSettings> settings,
        ILogService logService)
    {
        _logger =
            logger;

        _httpClient =
            httpClientFactory.CreateClient();

        _settings =
            settings.Value;

        _httpClient.Timeout =
            TimeSpan.FromSeconds(
                _settings.HttpTimeoutSeconds);

        _logService =
            logService;

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
    public async Task PublishAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        try
        {
            var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    _settings.CollectorUrl)
                {
                    Content =
                        JsonContent.Create(
                            snapshot)
                };

            if (!string.IsNullOrWhiteSpace(
                    _settings.ApiKey))
            {
                request.Headers.Add(
                    "X-API-Key",
                    _settings.ApiKey);
            }

            using var response =
                await _httpClient.SendAsync(
                    request,
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                await _logService.LogApi(
                    $"Snapshot upload failed. HTTP {(int)response.StatusCode} ({response.StatusCode})");

                response.EnsureSuccessStatusCode();
            }

            // -------------------------------------------------------------
            // First Successful Publish
            // -------------------------------------------------------------

            if (!_firstPublishSucceeded)
            {
                await _logService.LogAgent(
                    "First snapshot published successfully.");

                _firstPublishSucceeded =
                    true;
            }

            // -------------------------------------------------------------
            // Connectivity Restored
            // -------------------------------------------------------------

            if (_consecutiveFailures > 0)
            {
                await _logService.LogApi(
                    $"Connectivity restored after {_consecutiveFailures} consecutive failures.");

                _consecutiveFailures =
                    0;
            }
        }
        catch (TaskCanceledException ex)
            when (!cancellationToken.IsCancellationRequested)
        {
            _consecutiveFailures++;

            await _logService.LogApi(
                $"Snapshot upload timed out after {_settings.HttpTimeoutSeconds} seconds.");

            await _logService.LogError(
                "API",
                ex);

            throw;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _consecutiveFailures++;

            _logger.LogError(
                ex,
                "Failed to publish snapshot.");

            await _logService.LogError(
                "API",
                ex);

            // -------------------------------------------------------------
            // Failure Milestones
            // -------------------------------------------------------------

            if (_consecutiveFailures == 1)
            {
                await _logService.LogApi(
                    "Unable to communicate with monitoring API.");
            }

            if (_consecutiveFailures == 5 ||
                _consecutiveFailures == 10 ||
                _consecutiveFailures == 25 ||
                _consecutiveFailures == 50 ||
                _consecutiveFailures == 100)
            {
                await _logService.LogApi(
                    $"Consecutive publish failures: {_consecutiveFailures}");
            }

            throw;
        }
    }
}