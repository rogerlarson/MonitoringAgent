namespace MonitoringAgent.Api.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Data.Enums;
using MonitoringAgent.Api.Services.Interfaces;

public sealed class SnapshotCleanupService
    : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RetentionSettings _settings;
    private readonly ILogService _logService;

    public SnapshotCleanupService(
        IServiceProvider serviceProvider,
        IOptions<RetentionSettings> options,
        ILogService logService)
    {
        _serviceProvider = serviceProvider;
        _settings = options.Value;
        _logService = logService;
    }

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
                TimeSpan.FromHours(24),
                stoppingToken);
        }
    }

    private async Task Cleanup()
    {
        using var scope =
            _serviceProvider.CreateScope();

        var _db =
            scope.ServiceProvider
                .GetRequiredService<MonitoringDbContext>();

        await CleanupHostSnapshots(_db);
        await CleanupGatewaySnapshots(_db);
        await CleanupIgnitionSnapshots(_db);
        await CleanupAlertEvents(_db);
    }

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

    private async Task CleanupGatewaySnapshots(
    MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.GatewaySnapshotDays);

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
            $"Deleted {deleted} gateway snapshots");
    }

    private async Task CleanupIgnitionSnapshots(
    MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.IgnitionSnapshotDays);

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
            $"Deleted {deleted} ignition snapshots");
    }

    private async Task CleanupAlertEvents(
    MonitoringDbContext db)
    {
        var cutoff =
            DateTime.UtcNow.AddDays(
                -_settings.AlertEventDays);

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
            $"Deleted {deleted} alert events");
    }
}