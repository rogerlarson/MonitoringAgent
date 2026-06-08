/*
===============================================================================
Worker
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Simulates MonitoringAgent agents by periodically generating synthetic
monitoring snapshots and submitting them to the MonitoringAgent API.

Responsibilities:
- Load configured simulated servers
- Apply simulation profiles
- Generate health snapshots
- Submit snapshots to the Monitoring API
- Simulate offline and failure scenarios

Supported Profiles:
- Healthy
- HighCpu
- HighMemory
- DiskFull
- GatewayDown
- IgnitionDown
- Offline

Execution Flow:

    Configuration
          ↓
    Simulated Servers
          ↓
    Snapshot Generator
          ↓
    Monitoring API
          ↓
    Alert Engine Evaluation

Notes:
Each configured server behaves as an independent monitoring agent.

The simulator is intended for:
- Development environments
- Alert testing
- Dashboard validation
- Demonstrations
- Performance testing

Offline profiles intentionally stop reporting in order to exercise
snapshot-age alert rules and agent availability monitoring.

===============================================================================
*/

using Microsoft.Extensions.Options;
using MonitoringAgent.Simulator.Configuration;
using MonitoringAgent.Simulator.Enums;
using MonitoringAgent.Simulator.Models;
using System.Net.Http.Json;
using MonitoringAgent.Simulator.Services;

namespace MonitoringAgent.Simulator;

/// <summary>
/// Simulates MonitoringAgent agents by
/// periodically generating health snapshots
/// and posting them to the Monitoring API.
///
/// The simulator is configuration-driven and
/// supports multiple server profiles such as:
///
///     Healthy
///     HighCpu
///     HighMemory
///     DiskFull
///     GatewayDown
///     IgnitionDown
///     Offline
///
/// Each configured server behaves like an
/// independent monitoring agent.
/// </summary>
public sealed class Worker
    : BackgroundService
{
    // -------------------------------------------------------------------------
    // Dependencies
    // -------------------------------------------------------------------------

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SimulatorSettings _settings;

    /// <summary>
    /// Creates a new simulator worker.
    /// </summary>
    public Worker(
        IHttpClientFactory httpClientFactory,
        IOptions<SimulatorSettings> options)
    {
        _httpClientFactory =
            httpClientFactory;

        _settings =
            options.Value;
    }

    /// <summary>
    /// Main simulator execution loop.
    ///
    /// Responsibilities:
    /// - Load configured simulation profiles
    /// - Generate snapshots
    /// - Submit snapshots to the Monitoring API
    /// - Simulate offline agents
    /// - Repeat at configured intervals
    /// </summary>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        // ---------------------------------------------------------------------
        // Build Runtime Server Collection
        // ---------------------------------------------------------------------
        //
        // Convert configuration records into
        // runtime simulated server instances.
        //
        var servers =
            _settings.Servers
                .Select(x =>
                    new SimulatedServer
                    {
                        ServerName =
                            x.ServerName,

                        Profile =
                            Enum.Parse<
                                SimulationProfile>(
                                x.Profile,
                                true)
                    })
                .ToList();

        Console.WriteLine(
            $"Loaded {servers.Count} simulated servers.");

        // ---------------------------------------------------------------------
        // Configure API Client
        // ---------------------------------------------------------------------

        var httpClient =
            _httpClientFactory
                .CreateClient();

        httpClient.BaseAddress =
            new Uri(
                _settings.ApiUrl);

        if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            httpClient.DefaultRequestHeaders.Add(
                "X-API-Key",
                _settings.ApiKey);
        }

        // ---------------------------------------------------------------------
        // Main Simulation Loop
        // ---------------------------------------------------------------------

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                foreach (var server in servers)
                {
                    // ---------------------------------------------------------
                    // Offline Simulation
                    // ---------------------------------------------------------
                    //
                    // Offline profiles intentionally stop sending
                    // snapshots to simulate agent outages.
                    //
                    if (server.Profile ==
                        SimulationProfile.Offline)
                    {
                        continue;
                    }

                    // ---------------------------------------------------------
                    // Generate Snapshot
                    // ---------------------------------------------------------

                    var snapshot =
                        SnapshotGenerator.Create(
                            server);

                    // ---------------------------------------------------------
                    // Submit Snapshot
                    // ---------------------------------------------------------

                    var response =
                        await httpClient
                            .PostAsJsonAsync(
                                "/api/health",
                                snapshot,
                                stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(
                            $"{DateTime.Now:T} " +
                            $"{server.ServerName} " +
                            $"({server.Profile}) " +
                            $"posted successfully");
                    }
                    else
                    {
                        Console.WriteLine(
                            $"{DateTime.Now:T} " +
                            $"{server.ServerName} " +
                            $"failed: " +
                            $"{response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Simulator Error: {ex}");
            }

            // -----------------------------------------------------------------
            // Simulation Interval
            // -----------------------------------------------------------------

            await Task.Delay(
                TimeSpan.FromSeconds(
                    _settings.SnapshotIntervalSeconds),
                stoppingToken);
        }
    }
}