// ============================================================================
// Project: MonitoringAgent.Api
// File: AlertHistoryResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents historical alert information returned by the API.
//
//      Used by dashboards and reporting views to display alert lifecycle
//      details, occurrence counts, and associated rule information.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a historical alert record.
/// </summary>
public sealed class AlertHistoryResponse
{
    /// <summary>
    /// Unique identifier for the alert event.
    /// </summary>
    public long AlertEventId
    {
        get;
        set;
    }

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

    /// <summary>
    /// UTC timestamp when the alert was opened.
    /// </summary>
    public DateTime OpenedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the alert was closed.
    /// </summary>
    public DateTime? ClosedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the alert condition was first detected.
    /// </summary>
    public DateTime? FirstTriggeredUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Number of times the alert condition occurred.
    /// </summary>
    public int OccurrenceCount
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the alert rule that generated the alert.
    /// </summary>
    public string RuleName
    {
        get;
        set;
    }
        = string.Empty;
}