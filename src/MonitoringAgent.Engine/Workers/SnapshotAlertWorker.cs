// ============================================================================
// Project: MonitoringAgent
// File: SnapshotAlertWorker.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Background worker responsible for evaluating the most recent
//      monitoring snapshots collected from monitored servers.
//
//      Processes host, gateway, and Ignition snapshots, executes alert
//      rule evaluation, detects stale snapshot conditions, and records
//      worker health information used by engine monitoring services.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Data;
using MonitoringAgent.Engine.Configuration;
using MonitoringAgent.Engine.Services;

namespace MonitoringAgent.Engine.Workers;

/// <summary>
/// Periodically evaluates the latest snapshots for monitored servers and
/// executes alert rule processing.
/// </summary>
public sealed class SnapshotAlertWorker
    : BackgroundService
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly EngineSettings _engineSettings;

    // =====================================================================
    // Worker State
    // =====================================================================

    private readonly Guid _instanceId =
        Guid.NewGuid();

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes the snapshot alert worker.
    /// </summary>
    /// <param name="serviceProvider">
    /// Service provider used to create scoped dependencies.
    /// </param>
    /// <param name="logService">
    /// Logging service used for worker activity.
    /// </param>
    /// <param name="engineOptions">
    /// Engine configuration settings.
    /// </param>
    public SnapshotAlertWorker(
        IServiceProvider serviceProvider,
        ILogService logService,
        IOptions<EngineSettings> engineOptions)
    {
        _serviceProvider =
            serviceProvider;

        _logService =
            logService;

        _engineSettings =
            engineOptions.Value;
    }

    // =====================================================================
    // Worker Execution
    // =====================================================================

    /// <summary>
    /// Executes the alert monitoring loop until the worker is stopped.
    /// </summary>
    /// <param name="stoppingToken">
    /// Cancellation token used to stop the worker.
    /// </param>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        // Register worker startup.
        using (var startupScope =
            _serviceProvider.CreateScope())
        {
            var statusService =
                startupScope.ServiceProvider
                    .GetRequiredService<
                        EngineStatusService>();

            await statusService.RegisterStartup(
                nameof(
                    SnapshotAlertWorker),
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
                        .AsNoTracking()
                        .ToListAsync(
                            stoppingToken);

                await _logService.LogMaintenance(
                    $"SnapshotAlertMonitorService evaluating {servers.Count} servers");

                foreach (var server in servers)
                {
                    // -----------------------------------------------------
                    // Host Snapshot Evaluation
                    // -----------------------------------------------------

                    var hostSnapshot =
                        await db.HostSnapshots
                            .Where(x =>
                                x.ServerId ==
                                server.ServerId)
                            .OrderByDescending(x =>
                                x.SnapshotUtc)
                            .FirstOrDefaultAsync(
                                stoppingToken);

                    if (hostSnapshot != null)
                    {
                        await alertService
                            .EvaluateHostSnapshot(
                                server.ServerId,
                                hostSnapshot);

                        await alertService
                            .EvaluateSnapshotAge(
                                server.ServerId,
                                "Host Snapshot Stale",
                                hostSnapshot.SnapshotUtc,
                                "Host");
                    }

                    // -----------------------------------------------------
                    // Gateway Snapshot Evaluation
                    // -----------------------------------------------------

                    var gatewaySnapshot =
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

                    if (gatewaySnapshot != null)
                    {
                        await alertService
                            .EvaluateGatewaySnapshot(
                                server.ServerId,
                                gatewaySnapshot);

                        await alertService
                            .EvaluateSnapshotAge(
                                server.ServerId,
                                "Gateway Snapshot Stale",
                                gatewaySnapshot.SnapshotUtc,
                                "Gateway");
                    }

                    // -----------------------------------------------------
                    // Ignition Snapshot Evaluation
                    // -----------------------------------------------------

                    var ignitionSnapshot =
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

                    if (ignitionSnapshot != null)
                    {
                        await alertService
                            .EvaluateIgnitionSnapshot(
                                server.ServerId,
                                ignitionSnapshot);

                        await alertService
                            .EvaluateSnapshotAge(
                                server.ServerId,
                                "Ignition Snapshot Stale",
                                ignitionSnapshot.SnapshotUtc,
                                "Ignition");
                    }
                }

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
                        SnapshotAlertWorker),
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
                            SnapshotAlertWorker),
                        ex);
                }
                catch
                {
                    // Ignore status tracking failures while
                    // processing another exception.
                }

                await _logService.LogError(
                    "ALERT",
                    ex);
            }

            // Wait until the next monitoring cycle.
            await Task.Delay(
                TimeSpan.FromSeconds(
                    _engineSettings
                        .AlertMonitorIntervalSeconds),
                stoppingToken);
        }
    }

    // =====================================================================
    // Worker Shutdown
    // =====================================================================

    /// <summary>
    /// Records worker shutdown information before stopping.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token used during shutdown.
    /// </param>
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
                SnapshotAlertWorker));

        await base.StopAsync(
            cancellationToken);
    }
}