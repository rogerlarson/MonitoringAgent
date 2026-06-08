// ============================================================================
// Project: MonitoringAgent
// File: ServerEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a monitored server registered within the monitoring
//      platform.
//
//      Stores server identity information, current health status,
//      operating system details, agent metadata, and relationships to
//      collected snapshots and monitored services.
// ============================================================================

using MonitoringAgent.Common.Enums;

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a monitored server within the monitoring platform.
/// </summary>
public sealed class ServerEntity
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
    } = string.Empty;

    // =====================================================================
    // Status
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the server was last seen by the monitoring
    /// platform.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Current calculated server status.
    /// </summary>
    public ServerStatus Status
    {
        get;
        set;
    }

    // =====================================================================
    // Host Information
    // =====================================================================

    /// <summary>
    /// Operating system name.
    /// </summary>
    public string? OperatingSystem
    {
        get;
        set;
    }

    /// <summary>
    /// Operating system version.
    /// </summary>
    public string? OperatingSystemVersion
    {
        get;
        set;
    }

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
    // Agent Information
    // =====================================================================

    /// <summary>
    /// Monitoring agent version.
    /// </summary>
    public string? AgentVersion
    {
        get;
        set;
    }

    /// <summary>
    /// Windows domain name.
    /// </summary>
    public string? DomainName
    {
        get;
        set;
    }

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the server record was created.
    /// </summary>
    public DateTime CreatedDateUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Navigation Properties
    // =====================================================================

    /// <summary>
    /// Host snapshots collected for the server.
    /// </summary>
    public ICollection<HostSnapshotEntity> HealthSnapshots
    {
        get;
        set;
    } = new List<HostSnapshotEntity>();

    /// <summary>
    /// Monitored services associated with the server.
    /// </summary>
    public ICollection<ServerServiceEntity> ServerServices
    {
        get;
        set;
    } = new List<ServerServiceEntity>();
}