using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Services;
using MonitoringAgent.Api.Models.Responses;

namespace MonitoringAgent.Api.Controllers;

[ApiController]
[Route("api/servers")]
public sealed class ServersController
    : ControllerBase
{
    private readonly MonitoringDbContext _db;

    public ServersController(
        MonitoringDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns all servers.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetServers()
    {
        var servers =
            await _db.Servers
                .OrderBy(x => x.ServerName)
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

            var status =
                ServerStatusCalculator.Calculate(
                    server.LastSeenUtc,
                    latestSnapshot);

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
                        status,

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

        return Ok(result);
    }

    /// <summary>
    /// Returns a single server.
    /// </summary>
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

        var status =
            ServerStatusCalculator.Calculate(
                server.LastSeenUtc,
                latestSnapshot);

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
                     status,

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

    /// <summary>
    /// Returns latest snapshot for a server.
    /// </summary>
    [HttpGet("{serverId:int}/latest")]
    public async Task<IActionResult> GetLatest(
        int serverId)
    {
        var snapshot =
            await _db.HostSnapshots
                .Where(x => x.ServerId == serverId)
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

    /// <summary>
    /// Returns historical snapshots.
    /// </summary>
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

        if (intervalMinutes == null || intervalMinutes <= 1)
        {
            return Ok(
                snapshots.Select(
                    x => new HistoryPointResponse
                    {
                        TimestampUtc = x.SnapshotUtc,

                        CpuPercent = x.CpuPercent,
                        MemoryPercent = x.MemoryPercent,

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
        else
        {
            // Create buckets
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
                            x.SnapshotUtc.Ticks / ticks * ticks,
                            DateTimeKind.Utc);
                    })
                    .OrderBy(x => x.Key)
                    .ToList();

            // Average each bucket
            var result = buckets
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

            return Ok(result);
        }
    }

    [HttpGet("{serverId:int}/gateway-history")]
    public async Task<IActionResult> GetGatewayHistory(
    int serverId,
    int hours = 24,
    int? intervalMinutes = null)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(-hours);

        var snapshots =
            await _db.GatewaySnapshots
                .Where(x =>
                    x.ServerService.ServerId == serverId &&
                    x.SnapshotUtc >= cutoff)
                .OrderBy(x =>
                    x.SnapshotUtc)
                .ToListAsync();

        if (intervalMinutes == null || intervalMinutes <= 1)
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
        else
        {
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
                    .OrderBy(x => x.Key)
                    .ToList();
            
            var result =
                buckets.Select(x =>
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

            return Ok(result);
        }
    }

    [HttpGet("{serverId:int}/ignition-history")]
    public async Task<IActionResult> GetIgnitionHistory(
    int serverId,
    int hours = 24,
    int? intervalMinutes = null)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(-hours);

        var snapshots =
            await _db.IgnitionSnapshots
                .Where(x =>
                    x.ServerService.ServerId == serverId &&
                    x.SnapshotUtc >= cutoff)
                .OrderBy(x =>
                    x.SnapshotUtc)
                .ToListAsync();
            
            if (intervalMinutes == null || intervalMinutes <= 1)
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
        else
        {
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
                    .OrderBy(x => x.Key)
                    .ToList();
            
            var result =
                buckets.Select(x =>
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
                            x.Max(y =>
                                y.UptimeMinutes),

                        ProcessId =
                            x.Last().ProcessId
                    })
                .ToList();

            return Ok(result);
        }
    }

    /// <summary>
    /// Returns a summary of historical snapshots using average, max, min, etc.
    /// </summary>
    [HttpGet("{serverId:int}/history/summary")]
    public async Task<IActionResult> GetHistorySummary(
    int serverId,
    int hours = 24)
    {
        var cutoff =
            DateTime.UtcNow.AddHours(-hours);

        var query =
            _db.HostSnapshots
                .Where(x =>
                    x.ServerId == serverId &&
                    x.SnapshotUtc >= cutoff);

        var result =
            new HistorySummaryResponse
            {
                CpuAverage =
                    await query.AverageAsync(
                        x => x.CpuPercent),

                CpuMaximum =
                    await query.MaxAsync(
                        x => x.CpuPercent),

                MemoryAverage =
                    await query.AverageAsync(
                        x => x.MemoryPercent),

                MemoryMaximum =
                    await query.MaxAsync(
                        x => x.MemoryPercent),

                DiskAverage =
                    await query.AverageAsync(
                        x => x.DiskPercentUsed),

                DiskMaximum =
                    await query.MaxAsync(
                        x => x.DiskPercentUsed),

                DiskReadLatencyMaximum =
                    await query.MaxAsync(
                        x => x.DiskReadLatencyMs),

                DiskWriteLatencyMaximum =
                    await query.MaxAsync(
                        x => x.DiskWriteLatencyMs),

                AvgDiskQueueMaximum =
                    await query.MaxAsync(
                        x => x.AvgDiskQueueLength),

                NetworkInMaximum =
                    await query.MaxAsync(
                        x => x.NetworkBytesReceivedPerSec),

                NetworkOutMaximum =
                    await query.MaxAsync(
                        x => x.NetworkBytesSentPerSec)
            };

        return Ok(result);
    }

    /// <summary>
    /// Returns a summary of each server
    /// </summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var servers =
            await _db.Servers.ToListAsync();

        var healthy = 0;
        var warning = 0;
        var critical = 0;
        var offline = 0;

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

            var status =
                ServerStatusCalculator.Calculate(
                    server.LastSeenUtc,
                    latestSnapshot);

            switch (status)
            {
                case "Healthy":
                    healthy++;
                    break;

                case "Warning":
                    warning++;
                    break;

                case "Critical":
                    critical++;
                    break;

                case "Offline":
                    offline++;
                    break;
            }
        }

        return Ok(
            new ServerHealthSummaryResponse
            {
                Healthy =
                    healthy,

                Warning =
                    warning,

                Critical =
                    critical,

                Offline =
                    offline
            });
    }

    [HttpGet("{serverId:int}/alerts")]
    public async Task<IActionResult> GetAlerts(
        int serverId,
        int count = 20)
    {
        var alerts =
            await _db.AlertEvents
                .Where(x =>
                    x.ServerId == serverId)
                .OrderByDescending(x =>
                    x.OpenedUtc)
                .Take(count)
                .Select(x =>
                    new AlertHistoryResponse
                    {
                        AlertEventId =
                            x.AlertEventId,

                        Status =
                            x.Status.ToString(),

                        Message =
                            x.Message,

                        OpenedUtc =
                            DateTime.SpecifyKind(
                                x.OpenedUtc,
                                DateTimeKind.Utc),

                        ClosedUtc =
                            x.ClosedUtc == null
                                ? null
                                : DateTime.SpecifyKind(
                                    x.ClosedUtc.Value,
                                    DateTimeKind.Utc),

                        FirstTriggeredUtc =
                            x.FirstTriggeredUtc,

                        OccurrenceCount =
                            x.OccurrenceCount,

                        RuleName =
                            x.AlertRule.RuleName
                    })
                .ToListAsync();

        return Ok(alerts);
    }
}