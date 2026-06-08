// ============================================================================
// Project: MonitoringAgent.Api
// File: ServicesController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides service monitoring operations for server-associated services.
//
//      Supports service discovery, current status retrieval, and historical
//      monitoring data for services such as Ignition and Gateway.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides service monitoring operations.
/// </summary>
[ApiController]
[Route("api/services")]
public sealed class ServicesController
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
    public ServicesController(
        MonitoringDbContext db)
    {
        _db =
            db;
    }

    // =====================================================================
    // Service Catalog
    // =====================================================================

    /// <summary>
    /// Retrieves all configured service definitions.
    /// </summary>
    /// <returns>
    /// Collection of available services.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetServices()
    {
        var services =
            await _db.Services
                .Include(x =>
                    x.ServiceType)
                .OrderBy(x =>
                    x.ServiceName)
                .Select(x =>
                    new ServiceResponse
                    {
                        ServiceId =
                            x.ServiceId,

                        ServiceName =
                            x.ServiceName,

                        ServiceTypeName =
                            x.ServiceType
                                .ServiceTypeName
                    })
                .ToListAsync();

        return Ok(
            services);
    }

    // =====================================================================
    // Server Services
    // =====================================================================

    /// <summary>
    /// Retrieves services assigned to a server.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <returns>
    /// Collection of server services.
    /// </returns>
    [HttpGet("/api/servers/{serverId:int}/services")]
    public async Task<IActionResult> GetServerServices(
        int serverId)
    {
        var services =
            await _db.ServerServices
                .Include(x =>
                    x.Service)
                .Where(x =>
                    x.ServerId ==
                    serverId)
                .Select(x =>
                    new ServerServiceResponse
                    {
                        ServerServiceId =
                            x.ServerServiceId,

                        ServiceId =
                            x.ServiceId,

                        ServiceName =
                            x.Service.ServiceName,

                        Enabled =
                            x.Enabled
                    })
                .ToListAsync();

        return Ok(
            services);
    }

    // =====================================================================
    // Current Service Status
    // =====================================================================

    /// <summary>
    /// Retrieves the most recent status snapshot for a server service.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <param name="serverServiceId">
    /// Server service identifier.
    /// </param>
    /// <returns>
    /// Current service status.
    /// </returns>
    [HttpGet("/api/servers/{serverId:int}/services/{serverServiceId:int}/latest")]
    public async Task<IActionResult> GetLatestServiceStatus(
        int serverId,
        int serverServiceId)
    {
        var serverService =
            await _db.ServerServices
                .Include(x =>
                    x.Service)
                .FirstOrDefaultAsync(
                    x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.ServerId ==
                        serverId);

        if (serverService == null)
        {
            return NotFound();
        }

        // -----------------------------------------------------------------
        // Ignition Service
        // -----------------------------------------------------------------

        if (serverService.Service.ServiceName ==
            "Ignition")
        {
            var snapshot =
                await _db.IgnitionSnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId)
                    .OrderByDescending(x =>
                        x.SnapshotUtc)
                    .FirstOrDefaultAsync();

            if (snapshot == null)
            {
                return NotFound();
            }

            return Ok(
                new IgnitionStatusResponse
                {
                    ServerServiceId =
                        serverServiceId,

                    ServiceName =
                        "Ignition",

                    SnapshotUtc =
                        snapshot.SnapshotUtc,

                    Status =
                        snapshot.ProcessRunning
                            ? "Healthy"
                            : "Critical",

                    ServiceRunning =
                        snapshot.ServiceRunning,

                    ProcessRunning =
                        snapshot.ProcessRunning,

                    CpuPercent =
                        snapshot.CpuPercent,

                    MemoryMb =
                        snapshot.MemoryMb,

                    ThreadCount =
                        snapshot.ThreadCount,

                    HandleCount =
                        snapshot.HandleCount,

                    UptimeMinutes =
                        snapshot.UptimeMinutes,

                    ProcessId =
                        snapshot.ProcessId,

                    ProcessName =
                        snapshot.ProcessName,

                    IgnitionVersion =
                        snapshot.IgnitionVersion,

                    JavaVersion =
                        snapshot.JavaVersion
                });
        }

        // -----------------------------------------------------------------
        // Gateway Service
        // -----------------------------------------------------------------

        if (serverService.Service.ServiceName ==
            "Gateway")
        {
            var snapshot =
                await _db.GatewaySnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId)
                    .OrderByDescending(x =>
                        x.SnapshotUtc)
                    .FirstOrDefaultAsync();

            if (snapshot == null)
            {
                return NotFound();
            }

            return Ok(
                new GatewayStatusResponse
                {
                    ServerServiceId =
                        serverServiceId,

                    ServiceName =
                        "Gateway",

                    SnapshotUtc =
                        snapshot.SnapshotUtc,

                    Status =
                        snapshot.Reachable
                            ? "Healthy"
                            : "Critical",

                    Reachable =
                        snapshot.Reachable,

                    HttpStatusCode =
                        snapshot.HttpStatusCode,

                    ResponseMs =
                        snapshot.ResponseMs
                });
        }

        return NotFound();
    }

    // =====================================================================
    // Service History
    // =====================================================================

    /// <summary>
    /// Retrieves historical monitoring data for a server service.
    /// </summary>
    /// <param name="serverId">
    /// Server identifier.
    /// </param>
    /// <param name="serverServiceId">
    /// Server service identifier.
    /// </param>
    /// <param name="hours">
    /// Number of historical hours to retrieve.
    /// </param>
    /// <returns>
    /// Historical service metrics.
    /// </returns>
    [HttpGet("/api/servers/{serverId:int}/services/{serverServiceId:int}/history")]
    public async Task<IActionResult> GetServiceHistory(
        int serverId,
        int serverServiceId,
        int hours = 24)
    {
        var serverService =
            await _db.ServerServices
                .Include(x =>
                    x.Service)
                .FirstOrDefaultAsync(
                    x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.ServerId ==
                        serverId);

        if (serverService == null)
        {
            return NotFound();
        }

        var startUtc =
            DateTime.UtcNow.AddHours(
                -hours);

        // -----------------------------------------------------------------
        // Ignition History
        // -----------------------------------------------------------------

        if (serverService.Service.ServiceName ==
            "Ignition")
        {
            var history =
                await _db.IgnitionSnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.SnapshotUtc >=
                        startUtc)
                    .OrderBy(x =>
                        x.SnapshotUtc)
                    .Select(x =>
                        new IgnitionHistoryResponse
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
                        })
                    .Take(1000)
                    .ToListAsync();

            return Ok(
                history);
        }

        // -----------------------------------------------------------------
        // Gateway History
        // -----------------------------------------------------------------

        if (serverService.Service.ServiceName ==
            "Gateway")
        {
            var history =
                await _db.GatewaySnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.SnapshotUtc >=
                        startUtc)
                    .OrderBy(x =>
                        x.SnapshotUtc)
                    .Select(x =>
                        new GatewayHistoryResponse
                        {
                            SnapshotUtc =
                                x.SnapshotUtc,

                            Reachable =
                                x.Reachable,

                            HttpStatusCode =
                                x.HttpStatusCode,

                            ResponseMs =
                                x.ResponseMs
                        })
                    .Take(1000)
                    .ToListAsync();

            return Ok(
                history);
        }

        return NotFound();
    }
}