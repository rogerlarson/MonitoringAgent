// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerDetailsResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents detailed monitoring information for a specific server.
//
//      Combines server identity, operating system information, current
//      health metrics, monitored service status, and active alerts into
//      a single response model used by server detail views.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents detailed server monitoring information.
/// </summary>
public sealed class ServerDetailsResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the server.
    /// </summary>
    public int ServerId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the monitored server.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// UTC timestamp when the server last reported data.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Current calculated server status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = "Unknown";

    // =====================================================================
    // Registration Information
    // =====================================================================

    /// <summary>
    /// Number of monitored services assigned to the server.
    /// </summary>
    public int ServiceCount
    {
        get;
        set;
    }

    /// <summary>
    /// Windows domain associated with the server.
    /// </summary>
    public string DomainName
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Monitoring agent version currently installed.
    /// </summary>
    public string AgentVersion
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Host Information
    // =====================================================================

    /// <summary>
    /// Operating system name.
    /// </summary>
    public string OperatingSystem
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Operating system version.
    /// </summary>
    public string OperatingSystemVersion
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Number of logical processors.
    /// </summary>
    public int? ProcessorCount
    {
        get;
        set;
    }

    /// <summary>
    /// Total installed memory in megabytes.
    /// </summary>
    public long? TotalMemoryMb
    {
        get;
        set;
    }

    // =====================================================================
    // Current Metrics
    // =====================================================================

    /// <summary>
    /// Latest host performance metrics.
    /// </summary>
    public HostMetricsResponse Host
    {
        get;
        set;
    } = new();

    /// <summary>
    /// Latest gateway monitoring metrics.
    /// </summary>
    public GatewayMetricsResponse? Gateway
    {
        get;
        set;
    }

    /// <summary>
    /// Latest Ignition monitoring metrics.
    /// </summary>
    public IgnitionMetricsResponse? Ignition
    {
        get;
        set;
    }

    // =====================================================================
    // Alert Information
    // =====================================================================

    /// <summary>
    /// Active alerts currently associated with the server.
    /// </summary>
    public List<RecentAlertResponse> OpenAlerts
    {
        get;
        set;
    } = new();
}