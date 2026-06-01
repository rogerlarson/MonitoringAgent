using MonitoringAgent.Agent;
using MonitoringAgent.Agent.Collectors;
using MonitoringAgent.Agent.Collectors.Interfaces;
using MonitoringAgent.Agent.Configuration;
using Microsoft.Extensions.Hosting;
using MonitoringAgent.Agent.Services.Interfaces;
using MonitoringAgent.Agent.Services;
using System.Diagnostics;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Add the Windows Service...
        // services.AddWindowsService();

        // Add the Agent Settings from appsettings.json "AgentSettings" section...
        services.Configure<AgentSettings>(
            context.Configuration.GetSection("AgentSettings"));
        
        // Add HTTP Client...
        services.AddHttpClient();

        // Registrations...
        services.AddSingleton<IHealthPoster, HealthPoster>();
        services.AddSingleton<IMetricCollector, MetricCollector>();
        services.AddSingleton<SystemMetricsCollector>();
        services.AddSingleton<IgnitionMetricsCollector>();
        services.AddSingleton<GatewayMetricsCollector>();
        services.AddSingleton<CpuMemoryMetricsCollector>();
        services.AddSingleton<DiskMetricsCollector>();
        services.AddSingleton<DiskPerformanceMetricsCollector>();
        services.AddSingleton<NetworkMetricsCollector>();
        services.AddSingleton<HostInformationCollector>();

        // Add Worker service...
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();