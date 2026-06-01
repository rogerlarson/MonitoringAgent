using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Models.Responses;

namespace MonitoringAgent.Api.Controllers;

[ApiController]
[Route("api/services")]
public sealed class ServicesController
    : ControllerBase
{
    private readonly MonitoringDbContext _db;

    public ServicesController(
        MonitoringDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetServices()
    {
        var services =
            await _db.Services
                .Include(x => x.ServiceType)
                .OrderBy(x => x.ServiceName)
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

        return Ok(services);
    }

    [HttpGet("/api/servers/{serverId:int}/services")]
    public async Task<IActionResult> GetServerServices(
        int serverId)
    {
        var services =
            await _db.ServerServices
                .Include(x => x.Service)
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

        return Ok(services);
    }

    [HttpGet("/api/servers/{serverId:int}/services/{serverServiceId:int}/latest")]
    public async Task<IActionResult> GetLatestServiceStatus(
    int serverId,
    int serverServiceId)
    {
        var serverService =
            await _db.ServerServices
                .Include(x => x.Service)
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

    [HttpGet("/api/servers/{serverId:int}/services/{serverServiceId:int}/history")]
    public async Task<IActionResult> GetServiceHistory(
    int serverId,
    int serverServiceId,
    int hours = 24)
    {
        var serverService =
            await _db.ServerServices
                .Include(x => x.Service)
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

        if (serverService.Service.ServiceName ==
            "Ignition")
        {
            var history =
                await _db.IgnitionSnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.SnapshotUtc >= startUtc)
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

            return Ok(history);
        }

        if (serverService.Service.ServiceName ==
            "Gateway")
        {
            var history =
                await _db.GatewaySnapshots
                    .Where(x =>
                        x.ServerServiceId ==
                        serverServiceId
                        &&
                        x.SnapshotUtc >= startUtc)
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

            return Ok(history);
        }

        return NotFound();
    }
}