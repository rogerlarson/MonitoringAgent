// ============================================================================
// Project: MonitoringAgent
// File: SnapshotCleanupWorker.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Background worker responsible for enforcing data retention policies
//      for monitoring data stored in the database.
//
//      Periodically removes expired host snapshots, gateway snapshots,
//      Ignition snapshots, and alert events based on configured retention
//      settings to prevent unbounded database growth.
//
// Notes:
//      Uses ExecuteDeleteAsync() for efficient bulk deletion.
//      Avoids loading expired records into memory.
//      Supports graceful Windows Service shutdown through
//      cancellation-aware EF Core operations.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Data;
using MonitoringAgent.Engine.Configuration;

namespace MonitoringAgent.Engine.Workers;

/// <summary>
/// Periodically removes expired monitoring data according to configured
/// retention policies.
/// </summary>
public sealed class SnapshotCleanupWorker
    : BackgroundService
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly IServiceProvider _serviceProvider;
    private readonly RetentionSettings _settings;
    private readonly EngineSettings _engineSettings;
    private readonly ILogService _logService;

    // =====================================================================
    // Constructor
    // =====================================================================

    public SnapshotCleanupWorker(
        IServiceProvider serviceProvider,
        IOptions<RetentionSettings> options,
        IOptions<EngineSettings> engineOptions,
        ILogService logService)
    {
        _serviceProvider =
            serviceProvider;

        _settings =
            options.Value;

        _engineSettings =
            engineOptions.Value;

        _logService =
            logService;
    }

    // =====================================================================
    // Worker Execution
    // =====================================================================

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Cleanup(
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                await _logService.LogMaintenance(
                    $"Snapshot cleanup failed: {ex}");
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromMinutes(
                        _engineSettings
                            .SnapshotCleanupIntervalMinutes),
                    stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    // =====================================================================
    // Cleanup Operations
    // =====================================================================

    /// <summary>
    /// Executes all configured cleanup operations.
    /// </summary>
    private async Task Cleanup(
        CancellationToken stoppingToken)
    {
        using var scope =
            _serviceProvider.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<
                    MonitoringDbContext>();

        await CleanupHostSnapshots(
            db,
            stoppingToken);

        await CleanupGatewaySnapshots(
            db,
            stoppingToken);

        await CleanupIgnitionSnapshots(
            db,
            stoppingToken);

        await CleanupAlertEvents(
            db,
            stoppingToken);
    }

    // =====================================================================
    // Host Snapshot Cleanup
    // =====================================================================

    private async Task CleanupHostSnapshots(
        MonitoringDbContext db,
        CancellationToken stoppingToken)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.HostSnapshotDays);

        var deleted =
            await db.HostSnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ExecuteDeleteAsync(
                    stoppingToken);

        await _logService.LogMaintenance(
            $"Deleted {deleted} host snapshots");
    }

    // =====================================================================
    // Gateway Snapshot Cleanup
    // =====================================================================

    private async Task CleanupGatewaySnapshots(
        MonitoringDbContext db,
        CancellationToken stoppingToken)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.GatewaySnapshotDays);

        var deleted =
            await db.GatewaySnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ExecuteDeleteAsync(
                    stoppingToken);

        await _logService.LogMaintenance(
            $"Deleted {deleted} gateway snapshots");
    }

    // =====================================================================
    // Ignition Snapshot Cleanup
    // =====================================================================

    private async Task CleanupIgnitionSnapshots(
        MonitoringDbContext db,
        CancellationToken stoppingToken)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.IgnitionSnapshotDays);

        var deleted =
            await db.IgnitionSnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ExecuteDeleteAsync(
                    stoppingToken);

        await _logService.LogMaintenance(
            $"Deleted {deleted} ignition snapshots");
    }

    // =====================================================================
    // Alert Event Cleanup
    // =====================================================================

    private async Task CleanupAlertEvents(
        MonitoringDbContext db,
        CancellationToken stoppingToken)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.AlertEventDays);

        var deleted =
            await db.AlertEvents
                .Where(x =>
                    x.OpenedUtc < cutoff)
                .ExecuteDeleteAsync(
                    stoppingToken);

        await _logService.LogMaintenance(
            $"Deleted {deleted} alert events");
    }
}