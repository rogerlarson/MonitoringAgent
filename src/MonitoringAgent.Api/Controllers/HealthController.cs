using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Data.Entities;
using MonitoringAgent.Api.Data.Enums;
using MonitoringAgent.Api.Services;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController
    : ControllerBase
{
    private readonly ILogger<HealthController> _logger;
    private readonly MonitoringDbContext _db;
    private readonly AlertService _alertService;
    private readonly MonitoringSettings _settings;

    public HealthController(
        ILogger<HealthController> logger,
        MonitoringDbContext db,
        AlertService alertService,
        MonitoringSettings settings)
    {
        _logger = logger;
        _db = db;
        _alertService = alertService;
        _settings = settings;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
    [FromBody] HealthSnapshot snapshot)
    {
        // If API Key is required, check it, return unauthorized
        // if not present in headers or does not match
        if (_settings.RequireApiKey)
        {
            if (!Request.Headers.TryGetValue(
                    "X-API-Key",
                    out var apiKey))
            {
                return Unauthorized();
            }

            if (apiKey != _settings.ApiKey)
            {
                return Unauthorized();
            }
        }

        _logger.LogInformation(
            "Received snapshot from {ServerName}",
            snapshot.ServerName);

        _logger.LogInformation(
            "Database can connect: {Connected}",
            _db.Database.CanConnect());

        var server = await _db.Servers
            .FirstOrDefaultAsync(
                x => x.ServerName ==
                     snapshot.ServerName);

        if (server == null)
        {
            server = new ServerEntity
            {
                ServerName = snapshot.ServerName,
                CreatedDateUtc = DateTime.UtcNow,
                LastSeenUtc = DateTime.UtcNow,
                Status = "Unknown",
                OperatingSystem = snapshot.OperatingSystem,
                OperatingSystemVersion = snapshot.OperatingSystemVersion,
                ProcessorCount = snapshot.ProcessorCount,
                TotalMemoryMb = snapshot.TotalMemoryMb,
                AgentVersion = snapshot.AgentVersion,
                DomainName = snapshot.DomainName
            };

            _db.Servers.Add(server);

            await _db.SaveChangesAsync();
        }
        else
        {
            server.LastSeenUtc = DateTime.UtcNow;

            server.OperatingSystem =
                snapshot.OperatingSystem;

            server.OperatingSystemVersion =
                snapshot.OperatingSystemVersion;

            server.ProcessorCount =
                snapshot.ProcessorCount;

            server.TotalMemoryMb =
                snapshot.TotalMemoryMb;

            server.AgentVersion = snapshot.AgentVersion;

            server.DomainName = snapshot.DomainName;

            await _db.SaveChangesAsync();
        }

        // Check if the server services exist or not, add if not.. auto-registration...
        await EnsureServerServicesExist(server.ServerId);

        var entity = new HostSnapshotEntity
        {
            ServerId = server.ServerId,

            SnapshotUtc = snapshot.SnapshotUtc,

            // System

            CpuPercent = snapshot.CpuPercent,
            MemoryPercent = snapshot.MemoryPercent,
            AvailableMemoryMb = snapshot.AvailableMemoryMb,
            ProcessCount = snapshot.ProcessCount,
            SystemUptimeMinutes = snapshot.SystemUptimeMinutes,
            ContextSwitchesPerSec = snapshot.ContextSwitchesPerSec,
            PageFaultsPerSec = snapshot.PageFaultsPerSec,

            // Disk

            SystemDrive = snapshot.SystemDrive,
            DiskPercentUsed = snapshot.DiskPercentUsed,
            DiskFreeGb = snapshot.DiskFreeGb,
            DiskReadsPerSec = snapshot.DiskReadsPerSec,
            DiskWritesPerSec = snapshot.DiskWritesPerSec,
            DiskReadLatencyMs = snapshot.DiskReadLatencyMs,
            DiskWriteLatencyMs = snapshot.DiskWriteLatencyMs,
            DiskQueueLength = snapshot.DiskQueueLength,
            AvgDiskQueueLength = snapshot.AvgDiskQueueLength,

            // Network

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
                snapshot.TcpRetransmissionsPerSec,

            // Metadata

            CreatedDateUtc = DateTime.UtcNow
        };

        _db.HostSnapshots.Add(entity);

        var ignitionService =
        await _db.ServerServices
        .Include(x => x.Service)
        .FirstOrDefaultAsync(
            x =>
                x.ServerId ==
                server.ServerId
                &&
                x.Service.ServiceName ==
                "Ignition");

        if (ignitionService != null)
        {
            var ignitionEntity =
                new IgnitionSnapshotEntity
                {
                    ServerServiceId =
                        ignitionService.ServerServiceId,

                    SnapshotUtc =
                        snapshot.SnapshotUtc,

                    ServiceRunning =
                        snapshot.IgnitionServiceRunning,

                    ProcessRunning =
                        snapshot.IgnitionProcessRunning,

                    IgnitionVersion =
                        snapshot.IgnitionVersion,

                    JavaVersion =
                        snapshot.JavaVersion,

                    CpuPercent =
                        snapshot.IgnitionCpuPercent,

                    MemoryMb =
                        snapshot.IgnitionMemoryMb,

                    ThreadCount =
                        snapshot.IgnitionThreadCount,

                    HandleCount =
                        snapshot.IgnitionHandleCount,

                    UptimeMinutes =
                        snapshot.IgnitionUptimeMinutes,

                    ProcessId =
                        snapshot.IgnitionProcessId,

                    ProcessName =
                        snapshot.IgnitionProcessName,

                    CreatedDateUtc =
                        DateTime.UtcNow
                };

            await _alertService
                .EvaluateIgnitionSnapshot(
                    server.ServerId,
                    ignitionEntity);

            _db.IgnitionSnapshots
                .Add(ignitionEntity);
        }

        var gatewayService =
        await _db.ServerServices
        .Include(x => x.Service)
        .FirstOrDefaultAsync(
            x =>
                x.ServerId ==
                server.ServerId
                &&
                x.Service.ServiceName ==
                "Gateway");

        if (gatewayService != null)
        {
            var gatewayEntity =
                new GatewaySnapshotEntity
                {
                    ServerServiceId =
                        gatewayService.ServerServiceId,

                    SnapshotUtc =
                        snapshot.SnapshotUtc,

                    Reachable =
                        snapshot.GatewayReachable,

                    HttpStatusCode =
                        snapshot.GatewayHttpStatusCode,

                    ResponseMs =
                        snapshot.GatewayResponseMs,

                    CreatedDateUtc =
                        DateTime.UtcNow
                };

            await _alertService
                .EvaluateGatewaySnapshot(
                    server.ServerId,
                    gatewayEntity);

            _db.GatewaySnapshots
                .Add(gatewayEntity);
        }

        await _alertService
            .EvaluateHostSnapshot(
                server.ServerId,
                entity);

        // Calculate the server status for the database entry for this server...
        server.Status =
            ServerStatusCalculator.Calculate(
                server.LastSeenUtc,
                entity);

        await _db.SaveChangesAsync();

        return Ok();
    }

    private async Task EnsureServerServicesExist(
    int serverId)
    {
        var serviceIds =
            await _db.ServerServices
                .Where(x =>
                    x.ServerId ==
                    serverId)
                .Select(x =>
                    x.ServiceId)
                .ToListAsync();

        var missingServices =
            await _db.Services
                .Where(x =>
                    x.RegistrationMode ==
                    ServiceRegistrationMode.Global &&
                    !serviceIds.Contains(
                        x.ServiceId))
                .ToListAsync();

        foreach (var service in missingServices)
        {
            _db.ServerServices.Add(
                new ServerServiceEntity
                {
                    ServerId =
                        serverId,

                    ServiceId =
                        service.ServiceId,

                    ServiceInstanceName =
                        service.ServiceName,

                    Enabled =
                        true,

                    CreatedDateUtc =
                        DateTime.UtcNow
                });
        }
    }
}