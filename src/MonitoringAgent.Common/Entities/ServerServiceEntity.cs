// ============================================================================
// Project: MonitoringAgent
// File: ServerServiceEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the association between a monitored server and a
//      monitored service.
//
//      Server service records define which services are enabled for a
//      specific server and provide relationships to service-specific
//      monitoring data such as Ignition and gateway snapshots.
// ============================================================================

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a monitored service instance assigned to a server.
/// </summary>
public sealed class ServerServiceEntity
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the server service.
    /// </summary>
    public int ServerServiceId
    {
        get;
        set;
    }

    /// <summary>
    /// Associated server identifier.
    /// </summary>
    public int ServerId
    {
        get;
        set;
    }

    /// <summary>
    /// Associated service identifier.
    /// </summary>
    public int ServiceId
    {
        get;
        set;
    }

    // =====================================================================
    // Configuration
    // =====================================================================

    /// <summary>
    /// Optional service instance name used to distinguish multiple service
    /// instances on the same server.
    /// </summary>
    public string? ServiceInstanceName
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether monitoring is enabled for this service.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the server service record was created.
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
    /// Associated monitored server.
    /// </summary>
    public ServerEntity Server
    {
        get;
        set;
    } = null!;

    /// <summary>
    /// Associated service definition.
    /// </summary>
    public ServiceEntity Service
    {
        get;
        set;
    } = null!;

    /// <summary>
    /// Ignition snapshots collected for this service.
    /// </summary>
    public ICollection<IgnitionSnapshotEntity>
        IgnitionSnapshots
    {
        get;
        set;
    } = new List<IgnitionSnapshotEntity>();

    /// <summary>
    /// Gateway snapshots collected for this service.
    /// </summary>
    public ICollection<GatewaySnapshotEntity>
        GatewaySnapshots
    {
        get;
        set;
    } = new List<GatewaySnapshotEntity>();
}