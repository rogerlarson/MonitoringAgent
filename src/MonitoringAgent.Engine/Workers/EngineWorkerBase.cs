/*
===============================================================================
EngineWorkerBase
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Provides common functionality shared by MonitoringAgent
background workers.

Responsibilities:
- Worker startup registration
- Worker cycle registration
- Worker error registration
- Worker shutdown registration
- Worker lifecycle logging

Used By:
- SnapshotAlertWorker
- HostOfflineMonitorWorker
- Future engine workers

Lifecycle Support:

    Startup
        ↓
    RegisterStartup()

    Execution Cycle
        ↓
    RegisterCycle()

    Error Handling
        ↓
    RegisterError()

    Shutdown
        ↓
    RegisterShutdown()

Notes:
This base class centralizes worker telemetry and lifecycle
tracking to ensure consistent behavior across all engine
workers.

Workers inheriting from this class should focus solely on
their business logic and scheduling responsibilities.

===============================================================================
*/

using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Engine.Services;

namespace MonitoringAgent.Engine.Workers;

public abstract class EngineWorkerBase
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly ILogService LogService;

    protected Guid InstanceId
    {
        get;
    } = Guid.NewGuid();

    protected abstract string ServiceName
    {
        get;
    }

    protected EngineWorkerBase(
        IServiceProvider serviceProvider,
        ILogService logService)
    {
        ServiceProvider =
            serviceProvider;

        LogService =
            logService;
    }

    protected async Task RegisterStartup()
    {
        using var scope =
            ServiceProvider.CreateScope();

        var statusService =
            scope.ServiceProvider
                .GetRequiredService<
                    EngineStatusService>();

        await statusService.RegisterStartup(
            ServiceName,
            InstanceId);

        await LogService.LogMaintenance(
            $"[{ServiceName}] Started");
    }

    protected async Task RegisterCycle(
        DateTime startedUtc)
    {
        var durationMs =
            (long)(
                DateTime.UtcNow -
                startedUtc)
                .TotalMilliseconds;

        using var scope =
            ServiceProvider.CreateScope();

        var statusService =
            scope.ServiceProvider
                .GetRequiredService<
                    EngineStatusService>();

        await statusService.RegisterCycle(
            ServiceName,
            durationMs);
    }

    protected async Task RegisterError(
        Exception ex)
    {
        using var scope =
            ServiceProvider.CreateScope();

        var statusService =
            scope.ServiceProvider
                .GetRequiredService<
                    EngineStatusService>();

        await statusService.RegisterError(
            ServiceName,
            ex);

        await LogService.LogError(
            "ENGINE",
            ex);
    }

    protected async Task RegisterShutdown()
    {
        using var scope =
            ServiceProvider.CreateScope();

        var statusService =
            scope.ServiceProvider
                .GetRequiredService<
                    EngineStatusService>();

        await statusService.RegisterShutdown(
            ServiceName);

        await LogService.LogMaintenance(
            $"[{ServiceName}] Stopped");
    }

    protected async Task LogCycle(
        string message)
    {
        await LogService.LogMaintenance(
            $"[{ServiceName}] {message}");
    }
}