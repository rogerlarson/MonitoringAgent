// ============================================================================
// Project: MonitoringAgent
// File: AlertSeverity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the severity level assigned to an alert.
//
//      Alert severity indicates the potential impact of an alert condition
//      and is used to prioritize operator response, notification delivery,
//      dashboard presentation, and escalation workflows.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Represents the severity level assigned to an alert.
/// </summary>
public enum AlertSeverity
{
    /// <summary>
    /// Informational condition that does not indicate a problem but may be
    /// useful for visibility or operational awareness.
    /// </summary>
    Information = 1,

    /// <summary>
    /// Warning condition indicating a potential issue that requires
    /// attention but is not currently critical.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Critical condition indicating a significant issue that requires
    /// immediate attention.
    /// </summary>
    Critical = 3
}