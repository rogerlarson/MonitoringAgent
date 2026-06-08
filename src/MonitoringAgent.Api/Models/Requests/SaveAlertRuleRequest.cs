// ============================================================================
// Project: MonitoringAgent.Api
// File: SaveAlertRuleRequest.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Request model used to create or update alert rules.
//
//      Contains threshold, severity, notification, and lifecycle settings
//      used by the monitoring engine when evaluating alerts.
// ============================================================================

namespace MonitoringAgent.Api.Models.Requests;

/// <summary>
/// Represents a request to create or update an alert rule.
/// </summary>
public sealed class SaveAlertRuleRequest
{
    /// <summary>
    /// Display name of the alert rule.
    /// </summary>
    public string RuleName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Metric evaluated by the rule.
    /// </summary>
    public string MetricName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Comparison operator used during evaluation.
    /// </summary>
    public string Operator
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Threshold value used for comparison.
    /// </summary>
    public decimal? Threshold
    {
        get;
        set;
    }

    /// <summary>
    /// Alert severity level.
    /// </summary>
    public string Severity
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Number of seconds the condition must remain active
    /// before the alert opens.
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
    /// Indicates whether the alert should automatically
    /// close when the condition clears.
    /// </summary>
    public bool AutoClose
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

    /// <summary>
    /// Indicates whether the alert rule is enabled.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }
}