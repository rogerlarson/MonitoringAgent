// ============================================================================
// Project: MonitoringAgent.Api
// File: TopProblemServerResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a server with active alert activity.
//
//      Used by dashboard summary views to identify servers generating
//      the highest number of open alerts.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a server with active monitoring issues.
/// </summary>
public sealed class TopProblemServerResponse
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
    // Alert Information
    // =====================================================================

    /// <summary>
    /// Number of currently open alerts associated with the server.
    /// </summary>
    public int OpenAlertCount
    {
        get;
        set;
    }
}