// ============================================================================
// Project: MonitoringAgent.Agent
// File: Program.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Application entry point for the Monitoring Agent.
//
//      Configures dependency injection, application settings, HTTP
//      communication services, metric collectors, and hosted background
//      services required for agent operation.
// ============================================================================

using Microsoft.Extensions.Hosting;
using MonitoringAgent.Agent;
using MonitoringAgent.Agent.Collectors;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Configuration;
using MonitoringAgent.Agent.Services;
using MonitoringAgent.Agent.Services.Interfaces;

IHost host =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(
            (context, services) =>
            {
                // =========================================================
                // Windows Service Integration
                // =========================================================

                // Uncomment when deploying as a Windows service.
                // services.AddWindowsService();

                // =========================================================
                // Configuration
                // =========================================================

                // Bind AgentSettings from configuration.
                services.Configure<AgentSettings>(
                    context.Configuration.GetSection(
                        "AgentSettings"));

                // =========================================================
                // HTTP Services
                // =========================================================

                // HTTP client used for API communication.
                services.AddHttpClient();

                // =========================================================
                // Core Services
                // =========================================================

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
                    Worker>();
            })
        .Build();

await host.RunAsync();