using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Services.Interfaces;
using System.Runtime;
using Microsoft.Extensions.Options;

namespace MonitoringAgent.Api.Services;

public sealed class HostOfflineMonitorService
    : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogService _logService;
    private readonly MonitoringSettings _settings;

    public HostOfflineMonitorService(
        IServiceProvider serviceProvider,
        ILogService logService,
        IOptions<MonitoringSettings> options)
    {
        _serviceProvider = serviceProvider;
        _logService = logService;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope =
                    _serviceProvider
                        .CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<
                            MonitoringDbContext>();

                var alertService =
                    scope.ServiceProvider
                        .GetRequiredService<
                            AlertService>();

                var servers =
                    await db.Servers
                        .AsNoTracking()
                        .ToListAsync(
                            stoppingToken);

                foreach (
                    var server
                    in servers)
                {
                    bool offline =
                        server.LastSeenUtc == null ||
                        DateTime.UtcNow -
                        server.LastSeenUtc.Value >
                        TimeSpan.FromMinutes(
                            _settings.OfflineThresholdMinutes);

                    await _logService.LogHeartbeat(
                        $"ServerId={server.ServerId} Offline={offline} LastSeen={server.LastSeenUtc:u}");

                    await alertService
                        .EvaluateHeartbeat(
                            server.ServerId,
                            offline);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"HostOfflineMonitorService Error: {ex}");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(
                    _settings.HeartbeatCheckIntervalSeconds),
                stoppingToken);
        }
    }
}