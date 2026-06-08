// ============================================================================
// Project: MonitoringAgent
// File: ILogService.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines centralized logging operations used throughout the monitoring
//      platform.
//
//      Provides category-specific logging methods for monitoring activity,
//      operational events, email notifications, API interactions, heartbeat
//      processing, maintenance tasks, and exception reporting.
//
//      Implementations may write log entries to files, databases, external
//      logging systems, or other storage providers.
// ============================================================================

using MonitoringAgent.Common.Enums;

namespace MonitoringAgent.Common.Interfaces;

/// <summary>
/// Provides centralized logging operations for monitoring services.
/// </summary>
public interface ILogService
{
    // =====================================================================
    // Category Logging
    // =====================================================================

    /// <summary>
    /// Writes an alert-related log entry.
    /// </summary>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task LogAlert(
        string message);

    /// <summary>
    /// Writes an email-related log entry.
    /// </summary>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task LogEmail(
        string message);

    /// <summary>
    /// Writes a heartbeat-related log entry.
    /// </summary>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task LogHeartbeat(
        string message);

    /// <summary>
    /// Writes an API-related log entry.
    /// </summary>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task LogApi(
        string message);

    /// <summary>
    /// Writes a maintenance-related log entry.
    /// </summary>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task LogMaintenance(
        string message);

    // =====================================================================
    // General Logging
    // =====================================================================

    /// <summary>
    /// Writes a log entry for the specified category and severity level.
    /// </summary>
    /// <param name="category">
    /// Log category name.
    /// </param>
    /// <param name="level">
    /// Severity level of the log entry.
    /// </param>
    /// <param name="message">
    /// Log message to record.
    /// </param>
    Task Log(
        string category,
        LogLevel level,
        string message);

    // =====================================================================
    // Exception Logging
    // =====================================================================

    /// <summary>
    /// Writes an exception log entry.
    /// </summary>
    /// <param name="category">
    /// Log category name.
    /// </param>
    /// <param name="exception">
    /// Exception to record.
    /// </param>
    Task LogError(
        string category,
        Exception exception);
}