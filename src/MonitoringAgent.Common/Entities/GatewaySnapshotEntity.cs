// ============================================================================
// Project: MonitoringAgent
// File: GatewaySnapshotEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a gateway health snapshot collected from a monitored
//      service endpoint.
//
//      Gateway snapshots capture connectivity, HTTP response status, and
//      response time metrics used for availability monitoring, alerting,
//      and historical reporting.
// ============================================================================

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a gateway health snapshot collected from a monitored service.
/// </summary>
public sealed class GatewaySnapshotEntity
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the gateway snapshot.
    /// </summary>
    public long GatewaySnapshotId
    {
        get;
        set;
    }

    /// <summary>
    /// Associated monitored service identifier.
    /// </summary>
    public int ServerServiceId
    {
        get;
        set;
    }

    // =====================================================================
    // Snapshot Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the snapshot was collected.
    /// </summary>
    public DateTime SnapshotUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the gateway was reachable.
    /// </summary>
    public bool Reachable
    {
        get;
        set;
    }

    /// <summary>
    /// HTTP status code returned by the gateway.
    /// </summary>
    public int HttpStatusCode
    {
        get;
        set;
    }

    /// <summary>
    /// Gateway response time in milliseconds.
    /// </summary>
    public long ResponseMs
    {
        get;
        set;
    }

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the snapshot record was created.
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
    /// Associated monitored service.
    /// </summary>
    public ServerServiceEntity ServerService
    {
        get;
        set;
    } = null!;
}