// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerSummaryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a lightweight server summary used by dashboard and
//      overview endpoints.
//
//      Provides server availability information and active alert counts
//      without including detailed performance metrics.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a summarized monitored server.
/// </summary>
public sealed class ServerSummaryResponse
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

    // =====================================================================
    // Availability
    // =====================================================================

    /// <summary>
    /// Indicates whether the server is currently online.
    /// </summary>
    public bool Online
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the server last reported data.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Alert Information
    // =====================================================================

    /// <summary>
    /// Number of active alerts currently associated with the server.
    /// </summary>
    public int OpenAlertCount
    {
        get;
        set;
    }
}