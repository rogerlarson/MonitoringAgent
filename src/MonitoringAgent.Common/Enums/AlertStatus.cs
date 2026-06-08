// ============================================================================
// Project: MonitoringAgent
// File: AlertStatus.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the lifecycle state of an alert within the monitoring
//      platform.
//
//      Alert status values are used to track the progression of an alert
//      from initial detection through acknowledgement, suppression, and
//      eventual resolution.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Represents the current lifecycle state of an alert.
/// </summary>
public enum AlertStatus
{
    /// <summary>
    /// Alert has been detected but has not yet been fully processed.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Alert is active and requires attention.
    /// </summary>
    Open = 2,

    /// <summary>
    /// Alert has been acknowledged by an operator but remains active.
    /// </summary>
    Acknowledged = 3,

    /// <summary>
    /// Alert notifications have been suppressed while the alert remains
    /// active.
    /// </summary>
    Suppressed = 4,

    /// <summary>
    /// Alert condition has been resolved and the alert is no longer active.
    /// </summary>
    Closed = 5
}