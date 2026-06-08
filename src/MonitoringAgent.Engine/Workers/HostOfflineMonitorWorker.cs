/*
===============================================================================
HostOfflineMonitorWorker
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Monitors server heartbeat activity and calculates overall
server health status.

Responsibilities:
- Detect offline servers
- Evaluate heartbeat alerts
- Calculate overall server status
- Record server status changes
- Report worker execution metrics
- Maintain engine status information

Execution Flow:

    Servers
        ↓

    LastSeenUtc Evaluation
        ↓

    Offline Detection
        ↓

    EvaluateHeartbeat()
        ↓

    Latest Snapshots
        ↓

    ServerStatusCalculator
        ↓

    Status Update
        ↓

    Save Changes

Worker Lifecycle:

    Startup
        ↓
    RegisterStartup()

    Evaluation Cycle
        ↓
    RegisterCycle()

    Errors
        ↓
    RegisterError()

    Shutdown
        ↓
    RegisterShutdown()

Execution Frequency:
Configured through:

    EngineSettings.OfflineMonitorIntervalSeconds

Notes:
This worker determines the health status displayed
throughout the Monitoring Platform.

A server may be:

- Healthy
- Warning
- Critical
- Offline

Offline status always takes precedence over all
other calculated health conditions.

===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Data;
using MonitoringAgent.Engine.Calculators;
using MonitoringAgent.Engine.Configuration;
using MonitoringAgent.Engine.Services;

namespace MonitoringAgent.Engine.Workers;

///
/// Periodically evaluates server heartbeat status
/// and recalculates overall server health.
///
public sealed class HostOfflineMonitorWorker
: BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly MonitoringSettings _settings;
    private readonly EngineSettings _engineSettings;
    private readonly Guid _instanceId = Guid.NewGuid();

    public HostOfflineMonitorWorker(
        IServiceProvider serviceProvider,
        ILogService logService,
        IOptions<MonitoringSettings> options,
        IOptions<EngineSettings> engineOptions)
    {
        _serviceProvider =
            serviceProvider;

        _logService =
            logService;

        _settings =
            options.Value;

        _engineSettings =
            engineOptions.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        //
        // Register service startup.
        //
        using (var startupScope =
            _serviceProvider.CreateScope())
        {
            var statusService =
                startupScope.ServiceProvider
                    .GetRequiredService<
                        EngineStatusService>();

            await statusService.RegisterStartup(
                nameof(
                    HostOfflineMonitorWorker),
                _instanceId);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var cycleStartedUtc =
                DateTime.UtcNow;

            try
            {
                using var scope =
                    _serviceProvider
                        .CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<
                            MonitoringDbContext>();

                var alertService =
                    scope.ServiceProvider
                        .GetRequiredService<
                            AlertService>();

                var servers =
                    await db.Servers
                        .ToListAsync(
                            stoppingToken);

                await _logService.LogMaintenance(
                    $"HostOfflineMonitorService evaluating {servers.Count} servers");

                foreach (var server in servers)
                {
                    bool offline =
                        server.LastSeenUtc == null ||
                        DateTime.UtcNow -
                        server.LastSeenUtc.Value >
                        TimeSpan.FromMinutes(
                            _settings.OfflineThresholdMinutes);

                    //
                    // Heartbeat alert evaluation.
                    //
                    await alertService
                        .EvaluateHeartbeat(
                            server.ServerId,
                            offline);

                    //
                    // Most recent snapshots.
                    //
                    var latestHostSnapshot =
                        await db.HostSnapshots
                            .Where(x =>
                                x.ServerId ==
                                server.ServerId)
                            .OrderByDescending(x =>
                                x.SnapshotUtc)
                            .FirstOrDefaultAsync(
                                stoppingToken);

                    var latestGatewaySnapshot =
                        await db.GatewaySnapshots
                            .Include(x =>
                                x.ServerService)
                            .Where(x =>
                                x.ServerService.ServerId ==
                                server.ServerId)
                            .OrderByDescending(x =>
                                x.SnapshotUtc)
                            .FirstOrDefaultAsync(
                                stoppingToken);

                    var latestIgnitionSnapshot =
                        await db.IgnitionSnapshots
                            .Include(x =>
                                x.ServerService)
                            .Where(x =>
                                x.ServerService.ServerId ==
                                server.ServerId)
                            .OrderByDescending(x =>
                                x.SnapshotUtc)
                            .FirstOrDefaultAsync(
                                stoppingToken);

                    var previousStatus =
                        server.Status;

                    ServerStatus calculatedStatus;

                    if (offline)
                    {
                        calculatedStatus =
                            ServerStatus.Offline;
                    }
                    else
                    {
                        calculatedStatus =
                            ServerStatusCalculator.Calculate(
                                latestHostSnapshot,
                                latestGatewaySnapshot,
                                latestIgnitionSnapshot);
                    }

                    server.Status =
                        calculatedStatus;

                    if (previousStatus !=
                        calculatedStatus)
                    {
                        await _logService.LogMaintenance(
                            $"Server {server.ServerName} status changed from {previousStatus} to {calculatedStatus}");
                    }
                }

                //
                // Save all server updates once.
                //
                await db.SaveChangesAsync(
                    stoppingToken);

                //
                // Register cycle once.
                //
                var durationMs =
                    (long)(
                        DateTime.UtcNow -
                        cycleStartedUtc)
                        .TotalMilliseconds;

                var statusService =
                    scope.ServiceProvider
                        .GetRequiredService<
                            EngineStatusService>();

                await statusService.RegisterCycle(
                    nameof(
                        HostOfflineMonitorWorker),
                    durationMs);
            }
            catch (Exception ex)
            {
                try
                {
                    using var errorScope =
                        _serviceProvider
                            .CreateScope();

                    var statusService =
                        errorScope.ServiceProvider
                            .GetRequiredService<
                                EngineStatusService>();

                    await statusService.RegisterError(
                        nameof(
                            HostOfflineMonitorWorker),
                        ex);
                }
                catch
                {
                }

                await _logService.LogError(
                    "ENGINE",
                    ex);
            }

            await Task.Delay(
                TimeSpan.FromSeconds(
                    _engineSettings
                        .OfflineMonitorIntervalSeconds),
                stoppingToken);
        }
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        using var scope =
            _serviceProvider
                .CreateScope();

        var statusService =
            scope.ServiceProvider
                .GetRequiredService<
                    EngineStatusService>();

        await statusService.RegisterShutdown(
            nameof(
                HostOfflineMonitorWorker));

        await base.StopAsync(
            cancellationToken);
    }

}