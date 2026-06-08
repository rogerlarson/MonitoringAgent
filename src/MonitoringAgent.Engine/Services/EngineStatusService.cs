/*
===============================================================================
EngineStatusService
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Tracks runtime status information for MonitoringAgent
background workers.

Responsibilities:
- Register worker startup
- Register worker execution cycles
- Register worker errors
- Register worker shutdown
- Maintain worker telemetry

Tracked Information:
- Service status
- Instance identifier
- Startup time
- Last run time
- Last successful run
- Last error
- Run count
- Error count
- Execution duration

Used By:
- SnapshotAlertWorker
- HostOfflineMonitorWorker
- Future engine workers

Notes:
The Engine Status page relies on data maintained
by this service.

Each worker reports lifecycle events through
EngineStatusService, providing operational
visibility into engine activity.

===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Data;

namespace MonitoringAgent.Engine.Services;

public sealed class EngineStatusService
{
    private readonly MonitoringDbContext _db;

    public EngineStatusService(
        MonitoringDbContext db)
    {
        _db = db;
    }

    public async Task RegisterStartup(
        string serviceName,
        Guid instanceId)
    {
        var existing =
            await _db.EngineServices
                .FirstOrDefaultAsync(
                    x => x.ServiceName ==
                         serviceName);

        if (existing == null)
        {
            existing =
                new EngineServiceEntity
                {
                    ServiceName =
                        serviceName
                };

            _db.EngineServices.Add(
                existing);
        }

        existing.Status =
            "Running";

        existing.InstanceId =
            instanceId;

        existing.StartedUtc =
            DateTime.UtcNow;

        existing.LastError =
            null;

        existing.LastRunUtc =
            null;

        existing.LastSuccessUtc =
            null;

        existing.RunCount =
            0;

        existing.ErrorCount =
            0;

        existing.LastDurationMs =
            null;

        await _db.SaveChangesAsync();
    }

    public async Task RegisterCycle(
        string serviceName,
        long durationMs)
    {
        var service =
            await _db.EngineServices
                .FirstOrDefaultAsync(
                    x => x.ServiceName ==
                         serviceName);

        if (service == null)
        {
            return;
        }

        service.LastRunUtc =
            DateTime.UtcNow;

        service.LastSuccessUtc =
            DateTime.UtcNow;

        service.LastDurationMs =
            durationMs;

        service.RunCount++;

        await _db.SaveChangesAsync();
    }

    public async Task RegisterError(
        string serviceName,
        Exception ex)
    {
        var service =
            await _db.EngineServices
                .FirstOrDefaultAsync(
                    x => x.ServiceName ==
                         serviceName);

        if (service == null)
        {
            return;
        }

        service.ErrorCount++;

        service.LastError =
            ex.ToString();

        await _db.SaveChangesAsync();
    }

    public async Task RegisterShutdown(
        string serviceName)
    {
        var service =
            await _db.EngineServices
                .FirstOrDefaultAsync(
                    x => x.ServiceName ==
                         serviceName);

        if (service == null)
        {
            return;
        }

        service.Status =
            "Stopped";

        await _db.SaveChangesAsync();
    }
}