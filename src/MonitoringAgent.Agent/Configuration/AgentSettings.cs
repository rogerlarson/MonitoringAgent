// ============================================================================
// Project: MonitoringAgent.Agent
// File: AgentSettings.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines configuration settings used by the monitoring agent.
//
//      These settings control API connectivity, authentication, snapshot
//      collection behavior, monitored resources, Ignition integration,
//      and network communication parameters.
// ============================================================================

namespace MonitoringAgent.Agent.Configuration;

/// <summary>
/// Defines configuration settings used by the monitoring agent.
/// </summary>
public sealed class AgentSettings
{
    // =====================================================================
    // API Configuration
    // =====================================================================

    /// <summary>
    /// URL of the monitoring API used to receive health snapshots.
    /// </summary>
    public string CollectorUrl
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// API key used to authenticate the monitoring agent.
    /// </summary>
    public string ApiKey
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Timeout, in seconds, for outbound HTTP requests.
    /// </summary>
    public int HttpTimeoutSeconds
    {
        get;
        set;
    } = 10;

    // =====================================================================
    // Snapshot Collection
    // =====================================================================

    /// <summary>
    /// Frequency, in seconds, that health snapshots are collected.
    /// </summary>
    public int PollIntervalSeconds
    {
        get;
        set;
    } = 60;

    // =====================================================================
    // Ignition Configuration
    // =====================================================================

    /// <summary>
    /// Name of the Ignition Windows service.
    /// </summary>
    public string IgnitionServiceName
    {
        get;
        set;
    } = "Ignition";

    /// <summary>
    /// Ignition installation directory.
    /// </summary>
    public string IgnitionInstallPath
    {
        get;
        set;
    } = @"C:\Program Files\Inductive Automation\Ignition";

    /// <summary>
    /// URL used to verify Ignition gateway availability.
    /// </summary>
    public string GatewayUrl
    {
        get;
        set;
    } = "http://localhost:8088";

    // =====================================================================
    // Host Monitoring
    // =====================================================================

    /// <summary>
    /// Drive monitored for storage metrics.
    /// </summary>
    public string MonitoredDrive
    {
        get;
        set;
    } = "C:";

    /// <summary>
    /// Network adapter monitored for network metrics.
    /// </summary>
    public string NetworkInterfaceName
    {
        get;
        set;
    } = string.Empty;

    // =====================================================================
    // Validation
    // =====================================================================

    /// <summary>
    /// Validates configuration settings.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when configuration is invalid.
    /// </exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(
                CollectorUrl))
        {
            throw new InvalidOperationException(
                "AgentSettings: CollectorUrl is required.");
        }

        if (!Uri.TryCreate(
                CollectorUrl,
                UriKind.Absolute,
                out _))
        {
            throw new InvalidOperationException(
                "AgentSettings: CollectorUrl is not a valid URI.");
        }

        if (PollIntervalSeconds <= 0)
        {
            throw new InvalidOperationException(
                "AgentSettings: PollIntervalSeconds must be greater than zero.");
        }

        if (HttpTimeoutSeconds <= 0)
        {
            throw new InvalidOperationException(
                "AgentSettings: HttpTimeoutSeconds must be greater than zero.");
        }
    }
}