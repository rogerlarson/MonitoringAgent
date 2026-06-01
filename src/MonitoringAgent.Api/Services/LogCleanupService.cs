using Microsoft.Extensions.Options;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Services.Interfaces;

namespace MonitoringAgent.Api.Services;

public sealed class LogCleanupService
    : BackgroundService
{
    private readonly LogSettings _settings;
    private readonly ILogService _logService;

    public LogCleanupService(
        IOptions<LogSettings> settings,
        ILogService logService)
    {
        _settings =
            settings.Value;

        _logService =
            logService;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var logDirectory =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Logs");

                if (
                    Directory.Exists(
                        logDirectory))
                {
                    foreach (
                        var file
                        in Directory.GetFiles(
                            logDirectory,
                            "*.log"))
                    {
                        var info =
                            new FileInfo(
                                file);

                        if (
                            info.LastWriteTimeUtc <
                            DateTime.UtcNow.AddDays(
                                -_settings.RetentionDays))
                        {
                            File.Delete(
                                file);

                            await _logService
                                .LogMaintenance(
                                    $"Deleted old log file: {info.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _logService
                    .LogMaintenance(
                        $"Log cleanup error: {ex}");
            }

            await Task.Delay(
                TimeSpan.FromHours(24),
                stoppingToken);
        }
    }
}