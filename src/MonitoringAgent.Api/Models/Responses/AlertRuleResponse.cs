// ============================================================================
// Project: MonitoringAgent.Api
// File: AlertRuleResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents an alert rule returned by the API.
//
//      Includes rule configuration, threshold settings, severity,
//      notification behavior, and lifecycle options used by the
//      monitoring engine.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents an alert rule.
/// </summary>
public sealed class AlertRuleResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the alert rule.
    /// </summary>
    public int AlertRuleId
    {
        get;
        set;
    }

    /// <summary>
    /// Display name of the alert rule.
    /// </summary>
    public string RuleName
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Rule Definition
    // =====================================================================

    /// <summary>
    /// Metric evaluated by the alert rule.
    /// </summary>
    public string MetricName
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Comparison operator used during evaluation.
    /// </summary>
    public string Operator
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Threshold value used for comparison.
    /// </summary>
    public decimal? Threshold
    {
        get;
        set;
    }

    /// <summary>
    /// Severity level assigned when the rule is triggered.
    /// </summary>
    public string Severity
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Alert Behavior
    // =====================================================================

    /// <summary>
    /// Number of seconds the condition must remain active
    /// before an alert is opened.
    /// </summary>
    public int SustainSeconds
    {
        get;
        set;
    }

    /// <summary>
    /// Number of seconds between reminder notifications
    /// while the alert remains active.
    /// </summary>
    public int RepeatSeconds
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether the alert automatically closes
    /// when the condition clears.
    /// </summary>
    public bool AutoClose
    {
        get;
        set;
    }

    // =====================================================================
    // Status
    // =====================================================================

    /// <summary>
    /// Indicates whether the alert rule is enabled.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether email notifications are enabled.
    /// </summary>
    public bool EmailEnabled
    {
        get;
        set;
    }
}