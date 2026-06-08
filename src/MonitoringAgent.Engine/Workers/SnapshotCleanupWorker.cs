// ============================================================================
// Project: MonitoringAgent
// File: SnapshotCleanupWorker.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Background worker responsible for enforcing data retention policies
//      for monitoring data stored in the database.
//
//      Periodically removes expired host snapshots, gateway snapshots,
//      Ignition snapshots, and alert events based on configured retention
//      settings to prevent unbounded database growth.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Data;
using MonitoringAgent.Engine.Configuration;
using MonitoringAgent.Common.Interfaces;

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

    /// <summary>
    /// Initializes the snapshot cleanup worker.
    /// </summary>
    /// <param name="serviceProvider">
    /// Service provider used to create scoped dependencies.
    /// </param>
    /// <param name="options">
    /// Snapshot retention configuration settings.
    /// </param>
    /// <param name="engineOptions">
    /// Engine configuration settings.
    /// </param>
    /// <param name="logService">
    /// Logging service used for maintenance activity.
    /// </param>
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

    /// <summary>
    /// Executes the cleanup loop until the service is stopped.
    /// </summary>
    /// <param name="stoppingToken">
    /// Cancellation token used to stop the worker.
    /// </param>
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Cleanup();
            }
            catch (Exception ex)
            {
                await _logService.LogMaintenance(
                    $"Snapshot cleanup failed: {ex}");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(
                    _engineSettings
                        .SnapshotCleanupIntervalMinutes),
                stoppingToken);
        }
    }

    // =====================================================================
    // Cleanup Operations
    // =====================================================================

    /// <summary>
    /// Executes all configured cleanup operations.
    /// </summary>
    private async Task Cleanup()
    {
        using var scope =
            _serviceProvider.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<
                    MonitoringDbContext>();

        await CleanupHostSnapshots(db);
        await CleanupGatewaySnapshots(db);
        await CleanupIgnitionSnapshots(db);
        await CleanupAlertEvents(db);
    }

    // =====================================================================
    // Host Snapshot Cleanup
    // =====================================================================

    /// <summary>
    /// Removes host snapshots that exceed the configured retention period.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    private async Task CleanupHostSnapshots(
        MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.HostSnapshotDays);

        var snapshots =
            await db.HostSnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ToListAsync();

        var deleted =
            snapshots.Count;

        db.HostSnapshots.RemoveRange(
            snapshots);

        await db.SaveChangesAsync();

        await _logService.LogMaintenance(
            $"Deleted {deleted} host snapshots");
    }

    // =====================================================================
    // Gateway Snapshot Cleanup
    // =====================================================================

    /// <summary>
    /// Removes gateway snapshots that exceed the configured retention period.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    private async Task CleanupGatewaySnapshots(
        MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.GatewaySnapshotDays);

        var snapshots =
            await db.GatewaySnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ToListAsync();

        var deleted =
            snapshots.Count;

        db.GatewaySnapshots.RemoveRange(
            snapshots);

        await db.SaveChangesAsync();

        await _logService.LogMaintenance(
            $"Deleted {deleted} gateway snapshots");
    }

    // =====================================================================
    // Ignition Snapshot Cleanup
    // =====================================================================

    /// <summary>
    /// Removes Ignition snapshots that exceed the configured retention
    /// period.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    private async Task CleanupIgnitionSnapshots(
        MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.IgnitionSnapshotDays);

        var snapshots =
            await db.IgnitionSnapshots
                .Where(x =>
                    x.SnapshotUtc < cutoff)
                .ToListAsync();

        var deleted =
            snapshots.Count;

        db.IgnitionSnapshots.RemoveRange(
            snapshots);

        await db.SaveChangesAsync();

        await _logService.LogMaintenance(
            $"Deleted {deleted} ignition snapshots");
    }

    // =====================================================================
    // Alert Event Cleanup
    // =====================================================================

    /// <summary>
    /// Removes alert events that exceed the configured retention period.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    private async Task CleanupAlertEvents(
        MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.AlertEventDays);

        var alerts =
            await db.AlertEvents
                .Where(x =>
                    x.OpenedUtc < cutoff)
                .ToListAsync();

        var deleted =
            alerts.Count;

        db.AlertEvents.RemoveRange(
            alerts);

        await db.SaveChangesAsync();

        await _logService.LogMaintenance(
            $"Deleted {deleted} alert events");
    }
}