// ============================================================================
// Project: MonitoringAgent.Api
// File: GatewayMetricsResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the current Ignition Gateway status and performance
//      metrics for a monitored server.
//
//      Used by server detail views to display the latest gateway health
//      and connectivity information.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents current gateway monitoring metrics.
/// </summary>
public sealed class GatewayMetricsResponse
{
    // =====================================================================
    // Gateway Status
    // =====================================================================

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