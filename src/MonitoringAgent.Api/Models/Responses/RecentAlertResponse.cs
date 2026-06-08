// ============================================================================
// Project: MonitoringAgent.Api
// File: RecentAlertResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a recent alert returned by dashboard and monitoring
//      summary endpoints.
//
//      Provides alert status, severity, occurrence information, and
//      associated server details for recent alert activity views.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a recent monitoring alert.
/// </summary>
public sealed class RecentAlertResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the alert event.
    /// </summary>
    public long AlertEventId
    {
        get;
        set;
    }

    /// <summary>
    /// Identifier of the server associated with the alert.
    /// </summary>
    public int ServerId
    {
        get;
        set;
    }

    // =====================================================================
    // Alert Information
    // =====================================================================

    /// <summary>
    /// Name of the alert rule that generated the alert.
    /// </summary>
    public string RuleName
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Severity level of the alert.
    /// </summary>
    public string Severity
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Current alert status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Human-readable alert message.
    /// </summary>
    public string Message
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Timing
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the alert was opened.
    /// </summary>
    public DateTime OpenedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the alert condition was last observed.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Activity
    // =====================================================================

    /// <summary>
    /// Number of times the alert condition has occurred.
    /// </summary>
    public int OccurrenceCount
    {
        get;
        set;
    }

    /// <summary>
    /// Number of notifications sent for this alert.
    /// </summary>
    public int NotificationCount
    {
        get;
        set;
    }
}