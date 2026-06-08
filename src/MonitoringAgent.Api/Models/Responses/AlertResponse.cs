// ============================================================================
// Project: MonitoringAgent.Api
// File: AlertResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents detailed alert information returned by the API.
//
//      Includes alert lifecycle data, notification history, ownership
//      information, severity, and associated server metadata.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a monitoring alert.
/// </summary>
public sealed class AlertResponse
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
    /// Identifier of the alert rule that generated the alert.
    /// </summary>
    public int AlertRuleId
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

    /// <summary>
    /// Optional identifier of the server service associated with the alert.
    /// </summary>
    public int? ServerServiceId
    {
        get;
        set;
    }

    // =====================================================================
    // Alert State
    // =====================================================================

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
    /// Alert severity level.
    /// </summary>
    public string Severity
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
    /// UTC timestamp when the alert was closed.
    /// </summary>
    public DateTime? ClosedUtc
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

    /// <summary>
    /// UTC timestamp when the alert condition was first detected.
    /// </summary>
    public DateTime? FirstTriggeredUtc
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when the most recent notification was sent.
    /// </summary>
    public DateTime? LastNotificationUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Occurrences & Notifications
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

    // =====================================================================
    // Acknowledgement
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the alert was acknowledged.
    /// </summary>
    public DateTime? AcknowledgedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// User that acknowledged the alert.
    /// </summary>
    public string? AcknowledgedBy
    {
        get;
        set;
    }

    // =====================================================================
    // Suppression
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the alert was suppressed.
    /// </summary>
    public DateTime? SuppressedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// User that suppressed the alert.
    /// </summary>
    public string? SuppressedBy
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp when alert suppression expires.
    /// </summary>
    public DateTime? SuppressedUntilUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Closure
    // =====================================================================

    /// <summary>
    /// User that closed the alert.
    /// </summary>
    public string? ClosedBy
    {
        get;
        set;
    }

    // =====================================================================
    // Related Objects
    // =====================================================================

    /// <summary>
    /// Name of the server associated with the alert.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    }
        = string.Empty;

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