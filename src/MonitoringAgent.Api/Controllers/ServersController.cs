// ============================================================================
// Project: MonitoringAgent.Api
// File: ServersController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides server monitoring operations including current status,
//      historical metrics, service health, alert history, and server
//      summary information.
//
//      Exposes API endpoints used by the monitoring dashboard to display
//      server health, performance metrics, and alert activity.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides server monitoring operations.
/// </summary>
[ApiController]
[Route("api/servers")]
public sealed class ServersController
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
    public ServersController(
        MonitoringDbContext db)
    {
        _db =
            db;
    }

    // =====================================================================
    // Server Queries
    // =====================================================================

    /// <summary>
    /// Retrieves all monitored servers and their latest status.
    /// </summary>
    /// <returns>
    /// Collection of monitored servers.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetServers()
    {
        var servers =
            await _db.Servers
                .OrderBy(x =>
                    x.ServerName)
                .ToListAsync();

        var result =
            new List<ServerResponse>();

        foreach (var server in servers)
        {
            var latestSnapshot =
                await _db.HostSnapshots
                    .Where(x =>
                        x.ServerId ==
                        server.ServerId)
                    .OrderByDescending(
                        x => x.SnapshotUtc)
                    .FirstOrDefaultAsync();

            var latestIgnition =
                await _db.IgnitionSnapshots
                    .Where(x =>
                        x.ServerService.ServerId ==
                        server.ServerId)
                    .OrderByDescending(x =>
                        x.SnapshotUtc)
                    .FirstOrDefaultAsync();

            var latestGateway =
                await _db.GatewaySnapshots
                    .Where(x =>
                        x.ServerService.ServerId ==
                        server.ServerId)
                    .OrderByDescending(x =>
                        x.SnapshotUtc)
                    .FirstOrDefaultAsync();

            result.Add(
                new ServerResponse
                {
                    ServerId =
                        server.ServerId,

                    ServerName =
                        server.ServerName,

                    LastSeenUtc =
                        server.LastSeenUtc,

                    Status =
                        server.Status.ToString(),

                    CpuPercent =
                        latestSnapshot?.CpuPercent,

                    MemoryPercent =
                        latestSnapshot?.MemoryPercent,

                    DiskPercentUsed =
                        latestSnapshot?.DiskPercentUsed,

                    GatewayReachable =
                        latestGateway?.Reachable
                });
        }

        return Ok(
            result);
    }

    /// <summary>
    /// Retrieves detailed information for a specific server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <returns>
    /// Detailed server information.
    /// </returns>
    [HttpGet("{serverId:int}")]
    public async Task<IActionResult> GetServer(
        int serverId)
    {
        var server =
            await _db.Servers
                .FirstOrDefaultAsync(
                    x =>
                        x.ServerId ==
                        serverId);

        if (server == null)
        {
            return NotFound();
        }

        var latestSnapshot =
            await _db.HostSnapshots
                .Where(x =>
                    x.ServerId ==
                    serverId)
                .OrderByDescending(
                    x => x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var latestIgnition =
            await _db.IgnitionSnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId)
                .OrderByDescending(
                    x => x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var latestGateway =
            await _db.GatewaySnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId)
                .OrderByDescending(
                    x => x.SnapshotUtc)
                .FirstOrDefaultAsync();

        var serviceCount =
            await _db.ServerServices
                .CountAsync(x =>
                    x.ServerId ==
                    serverId &&
                    x.Enabled);

        return Ok(
            new ServerDetailsResponse
            {
                ServerId =
                    server.ServerId,

                ServerName =
                    server.ServerName,

                LastSeenUtc =
                    server.LastSeenUtc,

                Status =
                    server.Status.ToString(),

                ServiceCount =
                    serviceCount,

                DomainName =
                    server.DomainName,

                AgentVersion =
                    server.AgentVersion,

                OperatingSystem =
                    server.OperatingSystem,

                OperatingSystemVersion =
                    server.OperatingSystemVersion,

                ProcessorCount =
                    server.ProcessorCount,

                TotalMemoryMb =
                    server.TotalMemoryMb,

                Host =
                    latestSnapshot == null
                        ? new HostMetricsResponse()
                        : new HostMetricsResponse
                        {
                            CpuPercent =
                                latestSnapshot.CpuPercent,

                            MemoryPercent =
                                latestSnapshot.MemoryPercent,

                            DiskPercentUsed =
                                latestSnapshot.DiskPercentUsed,

                            AvailableMemoryMb =
                                latestSnapshot.AvailableMemoryMb,

                            SystemUptimeMinutes =
                                latestSnapshot.SystemUptimeMinutes
                        },

                Gateway =
                    latestGateway == null
                        ? null
                        : new GatewayMetricsResponse
                        {
                            Reachable =
                                latestGateway.Reachable,

                            HttpStatusCode =
                                latestGateway.HttpStatusCode,

                            ResponseMs =
                                latestGateway.ResponseMs
                        },

                Ignition =
                    latestIgnition == null
                        ? null
                        : new IgnitionMetricsResponse
                        {
                            ProcessRunning =
                                latestIgnition.ProcessRunning,

                            ServiceRunning =
                                latestIgnition.ServiceRunning,

                            IgnitionVersion =
                                latestIgnition.IgnitionVersion,

                            JavaVersion =
                                latestIgnition.JavaVersion,

                            CpuPercent =
                                latestIgnition.CpuPercent,

                            MemoryMb =
                                latestIgnition.MemoryMb,

                            UptimeMinutes =
                                latestIgnition.UptimeMinutes
                        }
            });
    }

    // =====================================================================
    // Current Snapshot Data
    // =====================================================================

    /// <summary>
    /// Retrieves the latest host snapshot for a server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <returns>
    /// Latest host snapshot.
    /// </returns>
    [HttpGet("{serverId:int}/latest")]
    public async Task<IActionResult> GetLatest(
        int serverId)
    {
        var snapshot =
            await _db.HostSnapshots
                .Where(x =>
                    x.ServerId == serverId)
                .OrderByDescending(
                    x => x.SnapshotUtc)
                .FirstOrDefaultAsync();

        if (snapshot == null)
        {
            return NotFound();
        }

        return Ok(
            new ServerLatestResponse
            {
                SnapshotUtc =
                    snapshot.SnapshotUtc,

                CpuPercent =
                    snapshot.CpuPercent,

                MemoryPercent =
                    snapshot.MemoryPercent,

                AvailableMemoryMb =
                    snapshot.AvailableMemoryMb,

                ProcessCount =
                    snapshot.ProcessCount,

                SystemUptimeMinutes =
                    snapshot.SystemUptimeMinutes,

                ContextSwitchesPerSec =
                    snapshot.ContextSwitchesPerSec,

                PageFaultsPerSec =
                    snapshot.PageFaultsPerSec,

                SystemDrive =
                    snapshot.SystemDrive,

                DiskPercentUsed =
                    snapshot.DiskPercentUsed,

                DiskFreeGb =
                    snapshot.DiskFreeGb,

                DiskReadsPerSec =
                    snapshot.DiskReadsPerSec,

                DiskWritesPerSec =
                    snapshot.DiskWritesPerSec,

                DiskReadLatencyMs =
                    snapshot.DiskReadLatencyMs,

                DiskWriteLatencyMs =
                    snapshot.DiskWriteLatencyMs,

                DiskQueueLength =
                    snapshot.DiskQueueLength,

                AvgDiskQueueLength =
                    snapshot.AvgDiskQueueLength,

                PrimaryNetworkInterface =
                    snapshot.PrimaryNetworkInterface,

                NetworkBytesReceivedPerSec =
                    snapshot.NetworkBytesReceivedPerSec,

                NetworkBytesSentPerSec =
                    snapshot.NetworkBytesSentPerSec,

                NetworkReceiveErrors =
                    snapshot.NetworkReceiveErrors,

                NetworkSendErrors =
                    snapshot.NetworkSendErrors,

                NetworkReceiveDiscards =
                    snapshot.NetworkReceiveDiscards,

                NetworkSendDiscards =
                    snapshot.NetworkSendDiscards,

                TcpRetransmissionsPerSec =
                    snapshot.TcpRetransmissionsPerSec
            });
    }

    // =====================================================================
    // Historical Data
    // =====================================================================

    /// <summary>
    /// Retrieves historical host snapshot data for a server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <param name="hours">
    /// Number of hours of history to retrieve.
    /// </param>
    /// <param name="intervalMinutes">
    /// Optional aggregation interval.
    /// </param>
    /// <returns>
    /// Historical host metrics.
    /// </returns>
    [HttpGet("{serverId:int}/history")]
    public async Task<IActionResult> GetHistory(
        int serverId,
        int hours = 24,
        int? intervalMinutes = null)
    {
        var cutoff =
            DateTime.UtcNow
                .AddHours(-hours);

        var snapshots =
            await _db.HostSnapshots
                .Where(x =>
                    x.ServerId == serverId &&
                    x.SnapshotUtc >= cutoff)
                .OrderBy(x =>
                    x.SnapshotUtc)
                .ToListAsync();

        if (intervalMinutes == null ||
            intervalMinutes <= 1)
        {
            return Ok(
                snapshots.Select(
                    x => new HistoryPointResponse
                    {
                        TimestampUtc =
                            x.SnapshotUtc,

                        CpuPercent =
                            x.CpuPercent,

                        MemoryPercent =
                            x.MemoryPercent,

                        DiskPercentUsed =
                            x.DiskPercentUsed,

                        DiskReadLatencyMs =
                            x.DiskReadLatencyMs,

                        DiskWriteLatencyMs =
                            x.DiskWriteLatencyMs,

                        AvgDiskQueueLength =
                            x.AvgDiskQueueLength,

                        DiskReadsPerSec =
                            x.DiskReadsPerSec,

                        DiskWritesPerSec =
                            x.DiskWritesPerSec,

                        NetworkBytesReceivedPerSec =
                            x.NetworkBytesReceivedPerSec,

                        NetworkBytesSentPerSec =
                            x.NetworkBytesSentPerSec
                    }));
        }

        var buckets =
            snapshots
                .GroupBy(x =>
                {
                    var ticks =
                        TimeSpan
                            .FromMinutes(
                                intervalMinutes.Value)
                            .Ticks;

                    return new DateTime(
                        x.SnapshotUtc.Ticks /
                        ticks *
                        ticks,
                        DateTimeKind.Utc);
                })
                .OrderBy(x =>
                    x.Key)
                .ToList();

        var result =
            buckets
                .Select(x =>
                    new HistoryPointResponse
                    {
                        TimestampUtc =
                            x.Key,

                        CpuPercent =
                            Math.Round(
                                x.Average(y =>
                                    y.CpuPercent),
                                2),

                        MemoryPercent =
                            Math.Round(
                                x.Average(y =>
                                    y.MemoryPercent),
                                2),

                        DiskPercentUsed =
                            Math.Round(
                                x.Average(y =>
                                    y.DiskPercentUsed),
                                2),

                        DiskReadLatencyMs =
                            Math.Round(
                                x.Average(y =>
                                    y.DiskReadLatencyMs),
                                2),

                        DiskWriteLatencyMs =
                            Math.Round(
                                x.Average(y =>
                                    y.DiskWriteLatencyMs),
                                2),

                        AvgDiskQueueLength =
                            Math.Round(
                                x.Average(y =>
                                    y.AvgDiskQueueLength),
                                2),

                        DiskReadsPerSec =
                            Math.Round(
                                x.Average(y =>
                                    y.DiskReadsPerSec),
                                2),

                        DiskWritesPerSec =
                            Math.Round(
                                x.Average(y =>
                                    y.DiskWritesPerSec),
                                2),

                        NetworkBytesReceivedPerSec =
                            Math.Round(
                                x.Average(y =>
                                    y.NetworkBytesReceivedPerSec),
                                2),

                        NetworkBytesSentPerSec =
                            Math.Round(
                                x.Average(y =>
                                    y.NetworkBytesSentPerSec),
                                2)
                    })
                .ToList();

        return Ok(
            result);
    }

    /// <summary>
    /// Retrieves historical gateway monitoring data.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <param name="hours">
    /// Number of hours of history to retrieve.
    /// </param>
    /// <param name="intervalMinutes">
    /// Optional aggregation interval.
    /// </param>
    /// <returns>
    /// Historical gateway metrics.
    /// </returns>
    [HttpGet("{serverId:int}/gateway-history")]
    public async Task<IActionResult> GetGatewayHistory(
        int serverId,
        int hours = 24,
        int? intervalMinutes = null)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(
                -hours);

        var snapshots =
            await _db.GatewaySnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId
                    &&
                    x.SnapshotUtc >= cutoff)
                .OrderBy(x =>
                    x.SnapshotUtc)
                .ToListAsync();

        if (intervalMinutes == null ||
            intervalMinutes <= 1)
        {
            return Ok(
                snapshots.Select(
                    x => new GatewayHistoryResponse
                    {
                        SnapshotUtc =
                            x.SnapshotUtc,

                        Reachable =
                            x.Reachable,

                        HttpStatusCode =
                            x.HttpStatusCode,

                        ResponseMs =
                            x.ResponseMs
                    }));
        }

        var buckets =
            snapshots
                .GroupBy(x =>
                {
                    var ticks =
                        TimeSpan
                            .FromMinutes(
                                intervalMinutes.Value)
                            .Ticks;

                    return new DateTime(
                        x.SnapshotUtc.Ticks /
                        ticks *
                        ticks,
                        DateTimeKind.Utc);
                })
                .OrderBy(x =>
                    x.Key)
                .ToList();

        var result =
            buckets
                .Select(x =>
                    new GatewayHistoryResponse
                    {
                        SnapshotUtc =
                            x.Key,

                        Reachable =
                            x.Any(y =>
                                y.Reachable),

                        HttpStatusCode =
                            x.Last().HttpStatusCode,

                        ResponseMs =
                            (long)Math.Round(
                                x.Average(y =>
                                    y.ResponseMs))
                    })
                .ToList();

        return Ok(
            result);
    }

    /// <summary>
    /// Retrieves historical Ignition monitoring data.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <param name="hours">
    /// Number of hours of history to retrieve.
    /// </param>
    /// <param name="intervalMinutes">
    /// Optional aggregation interval.
    /// </param>
    /// <returns>
    /// Historical Ignition metrics.
    /// </returns>
    [HttpGet("{serverId:int}/ignition-history")]
    public async Task<IActionResult> GetIgnitionHistory(
        int serverId,
        int hours = 24,
        int? intervalMinutes = null)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(
                -hours);

        var snapshots =
            await _db.IgnitionSnapshots
                .Where(x =>
                    x.ServerService.ServerId ==
                    serverId
                    &&
                    x.SnapshotUtc >= cutoff)
                .OrderBy(x =>
                    x.SnapshotUtc)
                .ToListAsync();

        if (intervalMinutes == null ||
            intervalMinutes <= 1)
        {
            return Ok(
                snapshots.Select(
                    x => new IgnitionHistoryResponse
                    {
                        SnapshotUtc =
                            x.SnapshotUtc,

                        ServiceRunning =
                            x.ServiceRunning,

                        ProcessRunning =
                            x.ProcessRunning,

                        CpuPercent =
                            x.CpuPercent,

                        MemoryMb =
                            x.MemoryMb,

                        ThreadCount =
                            x.ThreadCount,

                        HandleCount =
                            x.HandleCount,

                        UptimeMinutes =
                            x.UptimeMinutes,

                        ProcessId =
                            x.ProcessId
                    }));
        }

        var buckets =
            snapshots
                .GroupBy(x =>
                {
                    var ticks =
                        TimeSpan
                            .FromMinutes(
                                intervalMinutes.Value)
                            .Ticks;

                    return new DateTime(
                        x.SnapshotUtc.Ticks /
                        ticks *
                        ticks,
                        DateTimeKind.Utc);
                })
                .OrderBy(x =>
                    x.Key)
                .ToList();

        var result =
            buckets
                .Select(x =>
                    new IgnitionHistoryResponse
                    {
                        SnapshotUtc =
                            x.Key,

                        ServiceRunning =
                            x.Any(y =>
                                y.ServiceRunning),

                        ProcessRunning =
                            x.Any(y =>
                                y.ProcessRunning),

                        CpuPercent =
                            Math.Round(
                                x.Average(y =>
                                    y.CpuPercent),
                                2),

                        MemoryMb =
                            (long)Math.Round(
                                x.Average(y =>
                                    y.MemoryMb)),

                        ThreadCount =
                            (int)Math.Round(
                                x.Average(y =>
                                    y.ThreadCount)),

                        HandleCount =
                            (int)Math.Round(
                                x.Average(y =>
                                    y.HandleCount)),

                        UptimeMinutes =
                            (long)Math.Round(
                                x.Average(y =>
                                    y.UptimeMinutes)),

                        ProcessId =
                            x.Last().ProcessId
                    })
                .ToList();

        return Ok(
            result);
    }
}