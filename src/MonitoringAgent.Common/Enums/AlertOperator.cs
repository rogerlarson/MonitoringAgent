// ============================================================================
// Project: MonitoringAgent
// File: AlertOperator.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines comparison operators used by alert rules when evaluating
//      monitored values against configured thresholds.
//
//      Operators determine how collected metric values are compared to
//      rule thresholds during alert evaluation.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Defines comparison operators used during alert rule evaluation.
/// </summary>
public enum AlertOperator
{
    /// <summary>
    /// Value must be equal to the configured threshold.
    /// </summary>
    Equal = 1,

    /// <summary>
    /// Value must not be equal to the configured threshold.
    /// </summary>
    NotEqual = 2,

    /// <summary>
    /// Value must be greater than the configured threshold.
    /// </summary>
    GreaterThan = 3,

    /// <summary>
    /// Value must be greater than or equal to the configured threshold.
    /// </summary>
    GreaterThanOrEqual = 4,

    /// <summary>
    /// Value must be less than the configured threshold.
    /// </summary>
    LessThan = 5,

    /// <summary>
    /// Value must be less than or equal to the configured threshold.
    /// </summary>
    LessThanOrEqual = 6
}