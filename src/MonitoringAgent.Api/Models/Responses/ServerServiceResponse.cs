// ============================================================================
// Project: MonitoringAgent.Api
// File: ServerServiceResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a service assigned to a monitored server.
//
//      Used by server detail and service management views to display
//      registered services and their enabled state.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a service associated with a monitored server.
/// </summary>
public sealed class ServerServiceResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the server-service relationship.
    /// </summary>
    public int ServerServiceId
    {
        get;
        set;
    }

    /// <summary>
    /// Unique identifier of the service definition.
    /// </summary>
    public int ServiceId
    {
        get;
        set;
    }

    // =====================================================================
    // Service Information
    // =====================================================================

    /// <summary>
    /// Display name of the service.
    /// </summary>
    public string ServiceName
    {
        get;
        set;
    }
        = string.Empty;

    // =====================================================================
    // Configuration
    // =====================================================================

    /// <summary>
    /// Indicates whether monitoring is enabled for the service.
    /// </summary>
    public bool Enabled
    {
        get;
        set;
    }
}