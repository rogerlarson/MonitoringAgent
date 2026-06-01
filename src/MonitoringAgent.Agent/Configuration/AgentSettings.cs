

namespace MonitoringAgent.Agent.Configuration;

/// <summary>
/// Configuration settings used by the monitoring agent.
/// </summary>
public sealed class AgentSettings
{
    /// <summary>
    /// Base URL of the monitoring API.
    /// </summary>
    public string CollectorUrl { get; set; } = string.Empty;

    /// <summary>
    /// API key used for agent authentication.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Frequency, in seconds, that health snapshots are collected.
    /// </summary>
    public int PollIntervalSeconds { get; set; } = 60;

    /// <summary>
    /// Name of the Ignition Windows service.
    /// </summary>
    public string IgnitionServiceName { get; set; } = "Ignition";

    /// <summary>
    /// URL used to verify gateway availability.
    /// </summary>
    public string GatewayUrl { get; set; } = "http://localhost:8088";

    /// <summary>
    /// Timeout, in seconds, for outbound HTTP requests.
    /// </summary>
    public int HttpTimeoutSeconds { get; set; } = 10;

    /// <summary>
    /// Drive to monitor for storage metrics.
    /// </summary>
    public string MonitoredDrive { get; set; } = "C:";

    /// <summary>
    /// Network adapter to monitor.
    /// </summary>
    public string NetworkInterfaceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Ignition install path.
    /// </summary>
    public string IgnitionInstallPath { get; set; }
        = @"C:\Program Files\Inductive Automation\Ignition";
}
