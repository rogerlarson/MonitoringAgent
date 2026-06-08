// ============================================================================
// Project: MonitoringAgent.Api
// File: EngineServiceResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents the current status of a monitoring engine service.
//
//      Used by dashboard and administration views to display worker health,
//      execution statistics, and service activity information.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a monitoring engine service status record.
/// </summary>
public sealed class EngineServiceResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Name of the engine service.
    /// </summary>
    public string ServiceName
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Status
    // =====================================================================

    /// <summary>
    /// Current service status.
    /// </summary>
    public string Status
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Execution Statistics
    // =====================================================================

    /// <summary>
    /// Total number of execution cycles completed.
    /// </summary>
    public long RunCount
    {
        get;
        set;
    }

    /// <summary>
    /// Total number of execution errors encountered.
    /// </summary>
    public long ErrorCount
    {
        get;
        set;
    }

    /// <summary>
    /// UTC timestamp of the most recent successful execution.
    /// </summary>
    public DateTime? LastSuccessUtc
    {
        get;
        set;
    }
}