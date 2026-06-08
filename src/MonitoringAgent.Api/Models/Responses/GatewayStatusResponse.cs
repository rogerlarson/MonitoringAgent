// ============================================================================
// Project: MonitoringAgent.Api
// File: GatewayStatusResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the current status of a monitored gateway service.
//
//      Used by service detail views to display the latest gateway health,
//      availability, response time, and HTTP status information.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents the current gateway service status.
/// </summary>
public sealed class GatewayStatusResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Identifier of the monitored server service.
    /// </summary>
    public int ServerServiceId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the monitored service.
    /// </summary>
    public string ServiceName
    {
        get;
        set;
    }
        = string.Empty;

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

    // =====================================================================
    // Status
    // =====================================================================

    /// <summary>
    /// Current service status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = "Unknown";

    /// <summary>
    /// Indicates whether the gateway is reachable.
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
}