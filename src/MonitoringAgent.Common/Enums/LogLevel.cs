// ============================================================================
// Project: MonitoringAgent
// File: LogLevel.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the severity level of a log entry.
//
//      Log levels are used to categorize operational events, warnings,
//      errors, and critical failures throughout the monitoring platform.
//      These values may be used for filtering, reporting, alerting, and
//      log retention policies.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Represents the severity level of a log entry.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Informational message describing normal application behavior.
    /// </summary>
    Info = 1,

    /// <summary>
    /// Warning condition that may require attention but does not prevent
    /// normal operation.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Error condition indicating an operation failed or an unexpected
    /// condition occurred.
    /// </summary>
    Error = 3,

    /// <summary>
    /// Critical condition indicating a severe failure that may impact
    /// system availability or monitoring functionality.
    /// </summary>
    Critical = 4
}