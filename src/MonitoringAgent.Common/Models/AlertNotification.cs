// ============================================================================
// Project: MonitoringAgent
// File: AlertNotification.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents an alert notification payload generated from an active
//      alert and delivered to notification providers such as email,
//      Microsoft Teams, Slack, SMS, or other integrations.
//
//      Contains the information required to communicate alert state,
//      severity, occurrence counts, and timing information to operators.
// ============================================================================

namespace MonitoringAgent.Common.Models;

/// <summary>
/// Represents an alert notification payload generated from an active alert.
/// </summary>
public sealed class AlertNotification
{
    // =====================================================================
    // Alert Information
    // =====================================================================

    /// <summary>
    /// Name of the server that generated the alert.
    /// </summary>
    public string ServerName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Name of the alert rule that triggered.
    /// </summary>
    public string RuleName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Alert severity level.
    /// </summary>
    public string Severity
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Human-readable alert message.
    /// </summary>
    public string Message
    {
        get;
        set;
    } = string.Empty;

    // =====================================================================
    // Alert Lifecycle
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the alert was first opened.
    /// </summary>
    public DateTime OpenedUtc
    {
        get;
        set;
    }

    /// <summary>
    /// Number of times the alert condition has been detected.
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

    /// <summary>
    /// UTC timestamp when the alert condition was last observed.
    /// </summary>
    public DateTime? LastSeenUtc
    {
        get;
        set;
    }
}