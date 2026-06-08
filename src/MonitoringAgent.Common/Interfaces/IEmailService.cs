// ============================================================================
// Project: MonitoringAgent
// File: IEmailService.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines email notification operations for alert lifecycle events.
//
//      Implementations are responsible for generating notification content
//      and delivering email messages when alerts are opened, remain active,
//      or are resolved.
// ============================================================================

using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Common.Interfaces;

/// <summary>
/// Provides email notification operations for alert lifecycle events.
/// </summary>
public interface IEmailService
{
    // =====================================================================
    // Alert Notifications
    // =====================================================================

    /// <summary>
    /// Sends a notification indicating that an alert has been opened.
    /// </summary>
    /// <param name="notification">
    /// Alert notification details.
    /// </param>
    Task SendAlertOpened(
        AlertNotification notification);

    /// <summary>
    /// Sends a reminder notification for an alert that remains active.
    /// </summary>
    /// <param name="notification">
    /// Alert notification details.
    /// </param>
    Task SendAlertReminder(
        AlertNotification notification);

    /// <summary>
    /// Sends a notification indicating that an alert has been resolved.
    /// </summary>
    /// <param name="notification">
    /// Alert notification details.
    /// </param>
    Task SendAlertClosed(
        AlertNotification notification);
}