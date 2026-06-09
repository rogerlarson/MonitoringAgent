// ============================================================================
// Project: MonitoringAgent.Agent
// File: Program.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Application entry point for the Monitoring Agent.
//
//      Configures dependency injection, application settings, HTTP
//      communication services, logging services, metric collectors,
//      Windows Service hosting, and hosted background services required
//      for agent operation.
// ============================================================================

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MonitoringAgent.Agent;
using MonitoringAgent.Agent.Collectors;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services;
using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Services;
using MonitoringAgent.Common.Workers;

IHost host =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (context, services) =>
            {
                // =========================================================
                // Windows Service Integration
                // =========================================================

                services.AddWindowsService(
                    options =>
                    {
                        options.ServiceName =
                            "Monitoring Agent";
                    });

                // =========================================================
                // Configuration
                // =========================================================

                services.Configure<AgentSettings>(
                    context.Configuration.GetSection(
                        "AgentSettings"));

                services.Configure<LogSettings>(
                    context.Configuration.GetSection(
                        "Logging"));

                // =========================================================
                // HTTP Services
                // =========================================================

                services.AddHttpClient();

                // =========================================================
                // Core Services
                // =========================================================

                services.AddSingleton<
                    ILogService,
                    LogService>();

                services.AddSingleton<
                    IHealthPoster,
                    HealthPoster>();

                services.AddSingleton<
                    IMetricCollector,
                    MetricCollector>();

                // =========================================================
                // Metric Collectors
                // =========================================================

                services.AddSingleton<
                    SystemMetricsCollector>();

                services.AddSingleton<
                    IgnitionMetricsCollector>();

                services.AddSingleton<
                    GatewayMetricsCollector>();

                services.AddSingleton<
                    CpuMemoryMetricsCollector>();

                services.AddSingleton<
                    DiskMetricsCollector>();

                services.AddSingleton<
                    DiskPerformanceMetricsCollector>();

                services.AddSingleton<
                    NetworkMetricsCollector>();

                services.AddSingleton<
                    HostInformationCollector>();

                // =========================================================
                // Hosted Services
                // =========================================================

                services.AddHostedService<
                    AgentLifecycleService>();

                services.AddHostedService<
                    LogCleanupWorker>();

                services.AddHostedService<
                    Worker>();
            })
        .Build();

// ============================================================================
// Configuration Validation
// ============================================================================

using (var scope =
    host.Services.CreateScope())
{
    var settings =
        scope.ServiceProvider
            .GetRequiredService<
                IOptions<AgentSettings>>()
            .Value;

    settings.Validate();
}

await host.RunAsync();