// ============================================================================
// Project: MonitoringAgent.Api
// File: HealthController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Receives health snapshots from monitoring agents and persists
//      monitoring data to the database.
//
//      This endpoint is the primary ingestion point for host, gateway,
//      and Ignition monitoring data collected by deployed agents.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Models;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Receives and processes monitoring agent health snapshots.
/// </summary>
[ApiController]
[Route("api/health")]
public sealed class HealthController
    : ControllerBase
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly ILogger<HealthController> _logger;
    private readonly MonitoringDbContext _db;
    private readonly ApiSettings _settings;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the controller.
    /// </summary>
    /// <param name="logger">
    /// Logger instance.
    /// </param>
    /// <param name="db">
    /// Database context.
    /// </param>
    /// <param name="settings">
    /// API configuration settings.
    /// </param>
    public HealthController(
        ILogger<HealthController> logger,
        MonitoringDbContext db,
        ApiSettings settings)
    {
        _logger =
            logger;

        _db =
            db;

        _settings =
            settings;
    }

    // =====================================================================
    // Health Snapshot Ingestion
    // =====================================================================

    /// <summary>
    /// Receives a health snapshot from a monitoring agent.
    /// </summary>
    /// <param name="snapshot">
    /// Health snapshot payload.
    /// </param>
    /// <returns>
    /// Success response.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody]
        HealthSnapshot snapshot)
    {
        // -----------------------------------------------------------------
        // API Key Validation
        // -----------------------------------------------------------------

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

        // -----------------------------------------------------------------
        // Logging
        // -----------------------------------------------------------------

        _logger.LogInformation(
            "Received snapshot from {ServerName}",
            snapshot.ServerName);

        _logger.LogInformation(
            "Database can connect: {Connected}",
            _db.Database.CanConnect());

        // -----------------------------------------------------------------
        // Server Registration / Update
        // -----------------------------------------------------------------

        var server =
            await _db.Servers
                .FirstOrDefaultAsync(
                    x =>
                        x.ServerName ==
                        snapshot.ServerName);

        if (server == null)
        {
            server =
                new ServerEntity
                {
                    ServerName =
                        snapshot.ServerName,

                    CreatedDateUtc =
                        DateTime.UtcNow,

                    LastSeenUtc =
                        DateTime.UtcNow,

                    Status =
                        ServerStatus.Unknown,

                    OperatingSystem =
                        snapshot.OperatingSystem,

                    OperatingSystemVersion =
                        snapshot.OperatingSystemVersion,

                    ProcessorCount =
                        snapshot.ProcessorCount,

                    TotalMemoryMb =
                        snapshot.TotalMemoryMb,

                    AgentVersion =
                        snapshot.AgentVersion,

                    DomainName =
                        snapshot.DomainName
                };

            _db.Servers.Add(
                server);

            await _db.SaveChangesAsync();
        }
        else
        {
            server.LastSeenUtc =
                DateTime.UtcNow;

            server.OperatingSystem =
                snapshot.OperatingSystem;

            server.OperatingSystemVersion =
                snapshot.OperatingSystemVersion;

            server.ProcessorCount =
                snapshot.ProcessorCount;

            server.TotalMemoryMb =
                snapshot.TotalMemoryMb;

            server.AgentVersion =
                snapshot.AgentVersion;

            server.DomainName =
                snapshot.DomainName;

            await _db.SaveChangesAsync();
        }

        // -----------------------------------------------------------------
        // Service Auto Registration
        // -----------------------------------------------------------------

        await EnsureServerServicesExist(
            server.ServerId);

        // -----------------------------------------------------------------
        // Host Snapshot Creation
        // -----------------------------------------------------------------

        var entity =
            new HostSnapshotEntity
            {
                ServerId =
                    server.ServerId,

                SnapshotUtc =
                    snapshot.SnapshotUtc,

                // System

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

                // Disk

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

                CreatedDateUtc =
                    DateTime.UtcNow
            };

        _db.HostSnapshots.Add(
            entity);

        // -----------------------------------------------------------------
        // Ignition Snapshot Creation
        // -----------------------------------------------------------------

        var ignitionService =
            await _db.ServerServices
                .Include(x =>
                    x.Service)
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

            _db.IgnitionSnapshots.Add(
                ignitionEntity);
        }

        // -----------------------------------------------------------------
        // Gateway Snapshot Creation
        // -----------------------------------------------------------------

        var gatewayService =
            await _db.ServerServices
                .Include(x =>
                    x.Service)
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

            _db.GatewaySnapshots.Add(
                gatewayEntity);
        }

        // -----------------------------------------------------------------
        // Persist Snapshot Data
        // -----------------------------------------------------------------

        await _db.SaveChangesAsync();

        return Ok();
    }

    // =====================================================================
    // Service Registration Helpers
    // =====================================================================

    /// <summary>
    /// Ensures that all globally registered services exist for the
    /// specified server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
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
                    ServiceRegistrationMode.Global
                    &&
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

        await _db.SaveChangesAsync();
    }
}