// ============================================================================
// Project: MonitoringAgent.Api
// File: DashboardController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides dashboard and monitoring summary operations.
//
//      Exposes high-level monitoring data used by the dashboard including
//      server status, alert summaries, server health information, and
//      monitoring statistics.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides dashboard and monitoring summary operations.
/// </summary>
[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController
    : ControllerBase
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly MonitoringDbContext _db;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the controller.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    public DashboardController(
        MonitoringDbContext db)
    {
        _db =
            db;
    }

    // =====================================================================
    // Dashboard Summary
    // =====================================================================

    /// <summary>
    /// Retrieves high-level monitoring dashboard statistics.
    /// </summary>
    /// <returns>
    /// Dashboard summary information.
    /// </returns>
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

    // =====================================================================
    // Recent Alerts
    // =====================================================================

    /// <summary>
    /// Retrieves the most recent alert activity.
    /// </summary>
    /// <returns>
    /// Collection of recent alerts.
    /// </returns>
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

    // =====================================================================
    // Server Summary
    // =====================================================================

    /// <summary>
    /// Retrieves a summary of all monitored servers.
    /// </summary>
    /// <returns>
    /// Collection of server summary information.
    /// </returns>
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

    // =====================================================================
    // Server Health Summary
    // =====================================================================

    /// <summary>
    /// Retrieves aggregate server health statistics.
    /// </summary>
    /// <returns>
    /// Server health summary information.
    /// </returns>
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
                    .Include(x =>
                        x.AlertRule)
                    .AnyAsync(x =>
                        x.ServerId ==
                        server.ServerId &&
                        x.Status !=
                        AlertStatus.Closed &&
                        x.AlertRule!.Severity ==
                        AlertSeverity.Critical);

            if (hasCritical)
            {
                response.Critical++;
                continue;
            }

            var hasWarning =
                await _db.AlertEvents
                    .Include(x =>
                        x.AlertRule)
                    .AnyAsync(x =>
                        x.ServerId ==
                        server.ServerId &&
                        x.Status !=
                        AlertStatus.Closed &&
                        x.AlertRule!.Severity ==
                        AlertSeverity.Warning);

            if (hasWarning)
            {
                response.Warning++;
                continue;
            }

            response.Healthy++;
        }

        return Ok(
            response);
    }

    // =====================================================================
    // Server Details
    // =====================================================================

    /// <summary>
    /// Retrieves detailed monitoring information for a specific server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <returns>
    /// Detailed server information.
    /// </returns>
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

    // =====================================================================
    // Dashboard Trends
    // =====================================================================

    /// <summary>
    /// Retrieves dashboard trend and aggregate monitoring statistics.
    /// </summary>
    /// <param name="hours">
    /// Number of historical hours to evaluate.
    /// </param>
    /// <returns>
    /// Dashboard trend information.
    /// </returns>
    [HttpGet("trends")]
    public async Task<IActionResult> GetTrends(
        int hours = 24)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(
                -hours);

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
                    Severity =
                        x.Key,

                    Count =
                        x.Count()
                })
                .ToListAsync();

        var topProblemServers =
            await _db.AlertEvents
                .Where(x =>
                    x.Status !=
                    AlertStatus.Closed)
                .GroupBy(x =>
                    new
                    {
                        x.ServerId,
                        x.Server!.ServerName
                    })
                .Select(g =>
                    new TopProblemServerResponse
                    {
                        ServerId =
                            g.Key.ServerId,

                        ServerName =
                            g.Key.ServerName,

                        OpenAlertCount =
                            g.Count()
                    })
                .OrderByDescending(
                    x => x.OpenAlertCount)
                .Take(5)
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
                        x =>
                            x.Status ==
                            ServerStatus.Healthy),

                WarningServers =
                    await _db.Servers.CountAsync(
                        x =>
                            x.Status ==
                            ServerStatus.Warning),

                CriticalServers =
                    await _db.Servers.CountAsync(
                        x =>
                            x.Status ==
                            ServerStatus.Critical),

                OfflineServers =
                    await _db.Servers.CountAsync(
                        x =>
                            x.Status ==
                            ServerStatus.Offline),

                OpenAlerts =
                    await _db.AlertEvents
                        .CountAsync(
                            x =>
                                x.Status !=
                                AlertStatus.Closed),

                CriticalAlerts =
                    await _db.AlertEvents
                        .CountAsync(
                            x =>
                                x.Status !=
                                AlertStatus.Closed &&
                                x.AlertRule!.Severity ==
                                AlertSeverity.Critical),

                RunningWorkers =
                    await _db.EngineServices
                        .CountAsync(
                            x =>
                                x.Status ==
                                "Running"),

                StoppedWorkers =
                    await _db.EngineServices
                        .CountAsync(
                            x =>
                                x.Status !=
                                "Running"),

                TopProblemServers =
                    topProblemServers
            };

        return Ok(
            response);
    }
}