// ============================================================================
// Project: MonitoringAgent.Api
// File: ServiceResponse.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a monitoring service definition returned by the API.
//
//      Used by service management and configuration views to display
//      available services and their associated service types.
// ============================================================================

namespace MonitoringAgent.Api.Models.Responses;

/// <summary>
/// Represents a monitoring service definition.
/// </summary>
public sealed class ServiceResponse
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the service.
    /// </summary>
    public int ServiceId
    {
        get;
        set;
    }

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
    // Classification
    // =====================================================================

    /// <summary>
    /// Name of the service type associated with the service.
    /// </summary>
    public string ServiceTypeName
    {
        get;
        set;
    }
        = string.Empty;
}