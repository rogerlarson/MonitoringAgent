using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Data.Enums;
using MonitoringAgent.Api.Models.Responses;

namespace MonitoringAgent.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController
    : ControllerBase
{
    private readonly MonitoringDbContext _db;

    public DashboardController(
        MonitoringDbContext db)
    {
        _db = db;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var serverCount =
            await _db.Servers
                .CountAsync();

        var onlineServers =
            await _db.Servers
                .CountAsync(x =>
                    x.LastSeenUtc != null &&
                    x.LastSeenUtc >
                    DateTime.UtcNow.AddMinutes(-2));

        var offlineServers =
            serverCount -
            onlineServers;

        var openAlerts =
            await _db.AlertEvents
                .CountAsync(x =>
                    x.Status !=
                    AlertStatus.Closed);

        var criticalAlerts =
            await _db.AlertEvents
                .Include(x =>
                    x.AlertRule)
                .CountAsync(x =>
                    x.Status !=
                    AlertStatus.Closed &&
                    x.AlertRule!.Severity ==
                    AlertSeverity.Critical);

        var warningAlerts =
            await _db.AlertEvents
                .Include(x =>
                    x.AlertRule)
                .CountAsync(x =>
                    x.Status !=
                    AlertStatus.Closed &&
                    x.AlertRule!.Severity ==
                    AlertSeverity.Warning);

        return Ok(
            new DashboardSummaryResponse
            {
                ServerCount =
                    serverCount,

                OnlineServers =
                    onlineServers,

                OfflineServers =
                    offlineServers,

                OpenAlerts =
                    openAlerts,

                CriticalAlerts =
                    criticalAlerts,

                WarningAlerts =
                    warningAlerts
            });
    }

    [HttpGet("recent-alerts")]
    public async Task<IActionResult> GetRecentAlerts()
    {
        var alerts =
            await _db.AlertEvents
                .Include(x =>
                    x.AlertRule)
                .OrderByDescending(x =>
                    x.OpenedUtc)
                .Take(20)
                .Select(x =>
                    new RecentAlertResponse
                    {
                        AlertEventId =
                            x.AlertEventId,

                        ServerId =
                            x.ServerId,

                        RuleName =
                            x.AlertRule!.RuleName,

                        Severity =
                            x.AlertRule.Severity.ToString(),

                        Status =
                            x.Status.ToString(),

                        Message =
                            x.Message,

                        OpenedUtc =
                            x.OpenedUtc,

                        LastSeenUtc =
                            x.LastSeenUtc,

                        OccurrenceCount =
                            x.OccurrenceCount,

                        NotificationCount =
                            x.NotificationCount
                    })
                .ToListAsync();

        return Ok(
            alerts);
    }

    [HttpGet("servers")]
    public async Task<IActionResult> GetServers()
    {
        var cutoff =
            DateTime.UtcNow
                .AddMinutes(-2);

        var servers =
            await _db.Servers
                .Select(server =>
                    new ServerSummaryResponse
                    {
                        ServerId =
                            server.ServerId,

                        ServerName =
                            server.ServerName,

                        Online =
                            server.LastSeenUtc != null &&
                            server.LastSeenUtc >
                            cutoff,

                        LastSeenUtc =
                            server.LastSeenUtc,

                        OpenAlertCount =
                            _db.AlertEvents.Count(alert =>
                                alert.ServerId ==
                                server.ServerId &&
                                alert.Status !=
                                AlertStatus.Closed)
                    })
                .ToListAsync();

        return Ok(
            servers);
    }

    [HttpGet("server-health")]
    public async Task<IActionResult> GetServerHealth()
    {
        var cutoff =
            DateTime.UtcNow.AddMinutes(-2);

        var servers =
            await _db.Servers
                .ToListAsync();

        var response =
            new ServerHealthSummaryResponse();

        foreach (var server in servers)
        {
            var online =
                server.LastSeenUtc != null &&
                server.LastSeenUtc > cutoff;

            if (!online)
            {
                response.Offline++;
                continue;
            }

            var hasCritical =
                await _db.AlertEvents
                    .Include(x => x.AlertRule)
                    .AnyAsync(x =>
                        x.ServerId == server.ServerId &&
                        x.Status != AlertStatus.Closed &&
                        x.AlertRule!.Severity ==
                            AlertSeverity.Critical);

            if (hasCritical)
            {
                response.Critical++;
                continue;
            }

            var hasWarning =
                await _db.AlertEvents
                    .Include(x => x.AlertRule)
                    .AnyAsync(x =>
                        x.ServerId == server.ServerId &&
                        x.Status != AlertStatus.Closed &&
                        x.AlertRule!.Severity ==
                            AlertSeverity.Warning);

            if (hasWarning)
            {
                response.Warning++;
                continue;
            }

            response.Healthy++;
        }

        return Ok(response);
    }

    [HttpGet("server/{serverId:int}")]
    public async Task<IActionResult> GetServer(
    int serverId)
    {
        var server =
            await _db.Servers
                .FirstOrDefaultAsync(x =>
                    x.ServerId == serverId);

        if (server == null)
        {
            return NotFound();
        }

        var host =
            await _db.HostSnapshots
                .Where(x =>
                    x.ServerId == serverId)
                .OrderByDescending(x =>
                    x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var gateway =
            await _db.GatewaySnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId)
                .OrderByDescending(x =>
                    x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var ignition =
            await _db.IgnitionSnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId)
                .OrderByDescending(x =>
                    x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var alerts =
            await _db.AlertEvents
                .Include(x =>
                    x.AlertRule)
                .Where(x =>
                    x.ServerId == serverId &&
                    x.Status != AlertStatus.Closed)
                .ToListAsync();

        return Ok(
            new ServerDetailsResponse
            {
                Host =
                    new HostMetricsResponse
                    {
                        CpuPercent =
                            host?.CpuPercent ?? 0,

                        MemoryPercent =
                            host?.MemoryPercent ?? 0,

                        DiskPercentUsed =
                            host?.DiskPercentUsed ?? 0,

                        AvailableMemoryMb =
                            host?.AvailableMemoryMb ?? 0,

                        SystemUptimeMinutes =
                            host?.SystemUptimeMinutes ?? 0
                    },

                Gateway =
                    gateway == null
                        ? null
                        : new GatewayMetricsResponse
                        {
                            Reachable =
                                gateway.Reachable,

                            HttpStatusCode =
                                gateway.HttpStatusCode,

                            ResponseMs =
                                gateway.ResponseMs
                        },

                Ignition =
                    ignition == null
                        ? null
                        : new IgnitionMetricsResponse
                        {
                            ProcessRunning =
                                ignition.ProcessRunning,

                            ServiceRunning =
                                ignition.ServiceRunning,

                            IgnitionVersion =
                                ignition.IgnitionVersion,

                            JavaVersion =
                                ignition.JavaVersion,

                            CpuPercent =
                                ignition.CpuPercent,

                            MemoryMb =
                                ignition.MemoryMb,

                            UptimeMinutes =
                                ignition.UptimeMinutes
                        },

                OpenAlerts =
                    alerts.Select(x =>
                        new RecentAlertResponse
                        {
                            AlertEventId =
                                x.AlertEventId,

                            RuleName =
                                x.AlertRule!.RuleName,

                            Severity =
                                x.AlertRule.Severity.ToString(),

                            Status =
                                x.Status.ToString(),

                            Message =
                                x.Message
                        })
                        .ToList()
            });
    }

    [HttpGet("trends")]
    public async Task<IActionResult> GetTrends(
        int hours = 24)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(-hours);

        var hosts =
            _db.HostSnapshots
                .Where(x =>
                    x.SnapshotUtc >= cutoff);

        var gateways =
            _db.GatewaySnapshots
                .Where(x =>
                    x.SnapshotUtc >= cutoff);

        var alerts =
            _db.AlertEvents
                .Where(x =>
                    x.OpenedUtc >= cutoff);

        var alertCounts =
            await alerts
                .GroupBy(
                    x => x.AlertRule.Severity)
                .Select(x => new
                {
                    Severity = x.Key,
                    Count = x.Count()
                })
                .ToListAsync();

        var response =
            new DashboardTrendResponse
            {
                CpuAverage =
                    await hosts.AverageAsync(
                        x => x.CpuPercent),

                CpuMaximum =
                    await hosts.MaxAsync(
                        x => x.CpuPercent),

                MemoryAverage =
                    await hosts.AverageAsync(
                        x => x.MemoryPercent),

                MemoryMaximum =
                    await hosts.MaxAsync(
                        x => x.MemoryPercent),

                DiskAverage =
                    await hosts.AverageAsync(
                        x => x.DiskPercentUsed),

                DiskMaximum =
                    await hosts.MaxAsync(
                        x => x.DiskPercentUsed),

                GatewayResponseAverageMs =
                    await gateways.AnyAsync()
                        ? await gateways.AverageAsync(
                            x => (decimal)x.ResponseMs)
                        : 0,

                TotalAlertsOpened =
                    await alerts.CountAsync(),

                CriticalAlertsOpened =
                    alertCounts
                        .FirstOrDefault(
                            x =>
                                x.Severity ==
                                AlertSeverity.Critical)
                        ?.Count ?? 0,

                WarningAlertsOpened =
                    alertCounts
                        .FirstOrDefault(
                            x =>
                                x.Severity ==
                                AlertSeverity.Warning)
                        ?.Count ?? 0,

                HealthyServers =
                    await _db.Servers.CountAsync(
                        x => x.Status == "Healthy"),

                WarningServers =
                    await _db.Servers.CountAsync(
                        x => x.Status == "Warning"),

                CriticalServers =
                    await _db.Servers.CountAsync(
                        x => x.Status == "Critical"),

                OfflineServers =
                    await _db.Servers.CountAsync(
                        x => x.Status == "Offline")
            };

        return Ok(response);
    }
}