// ============================================================================
// Project : MonitoringAgent.Agent
// File    : HealthPoster.cs
//
// Purpose
// -------
// Temporary snapshot publisher.
//
// ============================================================================

using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Common.Models;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace MonitoringAgent.Agent.Services;

/// <summary>
/// Temporary health snapshot publisher.
/// </summary>
public sealed class HealthPoster : IHealthPoster
{
    private readonly ILogger<HealthPoster> _logger;
    private readonly HttpClient _httpClient;
    private readonly AgentSettings _settings;

    public HealthPoster(
        ILogger<HealthPoster> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AgentSettings> settings)
    {
        _logger = logger;

        _httpClient =
            httpClientFactory.CreateClient();

        _settings =
            settings.Value;

        // Log if API Key configured or not...
        _logger.LogInformation(
            "API key configured: {Configured}",
            !string.IsNullOrWhiteSpace(
            _settings.ApiKey));
    }

    public async Task PublishAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken)
    {
        //_logger.LogInformation(
        //    "Publishing snapshot for {ServerName}",
        //    snapshot.ServerName);
        _logger.LogInformation(
            "Server={ServerName} " +
            "Cpu={CpuPercent}% " +
            "Memory={MemoryPercent}% " +
            "AvailableMemoryMb={AvailableMemoryMb} " +
            "Processes={ProcessCount} " +
            "Uptime={SystemUptimeMinutes} " +
            "ContextSwitches={ContextSwitchesPerSec} " +
            "PageFaults={PageFaultsPerSec} " +
            "IgnitionService={IgnitionServiceRunning} " +
            "IgnitionProcess={IgnitionProcessRunning} " +
            "IgnitionMemoryMb={IgnitionMemoryMb} " +
            "IgnitionThreads={IgnitionThreadCount} " +
            "IgnitionHandles={IgnitionHandleCount} " +
            "IgnitionPid={IgnitionProcessId} " +
            "IgnitionCpu={IgnitionCpuPercent:F6}% " +
            "GatewayReachable={GatewayReachable} " +
            "GatewayResponseMs={GatewayResponseMs} " +
            "Drive={SystemDrive} " +
            "DiskUsed={DiskPercentUsed}% " +
            "DiskFreeGb={DiskFreeGb} " +
            "DiskReads={DiskReadsPerSec} " +
            "DiskWrites={DiskWritesPerSec} " +
            "DiskQueue={DiskQueueLength} " +
            "NetIn={NetworkBytesReceivedPerSec} " +
            "NetOut={NetworkBytesSentPerSec} " +
            "TcpRetrans={TcpRetransmissionsPerSec} " +
            "OS={OperatingSystem} " +
            "OSVersion={OperatingSystemVersion} " +
            "Cores={ProcessorCount} " +
            "TotalMemoryMb={TotalMemoryMb} " +
            "AvgDiskQueueLength={AvgDiskQueueLength} ",
            snapshot.ServerName,
            snapshot.CpuPercent,
            snapshot.MemoryPercent,
            snapshot.AvailableMemoryMb,
            snapshot.ProcessCount,
            snapshot.SystemUptimeMinutes,
            snapshot.ContextSwitchesPerSec,
            snapshot.PageFaultsPerSec,
            snapshot.IgnitionServiceRunning,
            snapshot.IgnitionProcessRunning,
            snapshot.IgnitionMemoryMb,
            snapshot.IgnitionThreadCount,
            snapshot.IgnitionHandleCount,
            snapshot.IgnitionProcessId,
            snapshot.IgnitionCpuPercent,
            snapshot.GatewayReachable,
            snapshot.GatewayResponseMs,
            snapshot.SystemDrive,
            snapshot.DiskPercentUsed,
            snapshot.DiskFreeGb,
            snapshot.DiskReadsPerSec,
            snapshot.DiskWritesPerSec,
            snapshot.DiskQueueLength,
            snapshot.NetworkBytesReceivedPerSec,
            snapshot.NetworkBytesSentPerSec,
            snapshot.TcpRetransmissionsPerSec,
            snapshot.OperatingSystem,
            snapshot.OperatingSystemVersion,
            snapshot.ProcessorCount,
            snapshot.TotalMemoryMb,
            snapshot.AvgDiskQueueLength
        );

        try
        {
            // TODO: Debug message for POST command to /api/health
            //_logger.LogInformation(
            //    "Posting snapshot to {CollectorUrl}",
            //    _settings.CollectorUrl);

            // Create the request variable object
            var request =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    _settings.CollectorUrl)
                {
                    Content =
                        JsonContent.Create(
                            snapshot)
                };

            // Add API Key to headers for authentication...
            if (!string.IsNullOrWhiteSpace(
                    _settings.ApiKey))
            {
                request.Headers.Add(
                    "X-API-Key",
                    _settings.ApiKey);
            }
            
            // Perform POST request and get the response...
            var response =
                await _httpClient.SendAsync(
                    request,
                    cancellationToken);

            // TODO: Debug message that POST completed and returned a status code...
            //_logger.LogInformation(
            //    "POST returned {StatusCode}",
            //    response.StatusCode);

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
