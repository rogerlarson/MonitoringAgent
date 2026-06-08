// ============================================================================
// Project: MonitoringAgent.Api
// File: GatewayHistoryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a historical Ignition Gateway monitoring data point.
//
//      Used for charting gateway availability, response times, and HTTP
//      status information over time.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a historical gateway monitoring record.
/// </summary>
public sealed class GatewayHistoryResponse
{
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
    // Gateway Status
    // =====================================================================

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
}