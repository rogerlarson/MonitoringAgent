// ============================================================================
// Project: MonitoringAgent
// File: ServiceEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a service definition that can be monitored by the
//      monitoring platform.
//
//      Service definitions describe the type of service being monitored,
//      the collector responsible for gathering metrics, and how the service
//      is registered with monitored servers.
// ============================================================================

using MonitoringAgent.Common.Enums;

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a service definition available for monitoring.
/// </summary>
public sealed class ServiceEntity
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
    } = string.Empty;

    // =====================================================================
    // Service Configuration
    // =====================================================================

    /// <summary>
    /// Associated service type identifier.
    /// </summary>
    public int ServiceTypeId
    {
        get;
        set;
    }

    /// <summary>
    /// Name of the collector responsible for gathering service metrics.
    /// </summary>
    public string? CollectorName
    {
        get;
        set;
    }

    /// <summary>
    /// Registration mode used when assigning the service to servers.
    /// </summary>
    public ServiceRegistrationMode RegistrationMode
    {
        get;
        set;
    }

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the service definition was created.
    /// </summary>
    public DateTime CreatedDateUtc
    {
        get;
        set;
    }

    // =====================================================================
    // Navigation Properties
    // =====================================================================

    /// <summary>
    /// Associated service type.
    /// </summary>
    public ServiceTypeEntity ServiceType
    {
        get;
        set;
    } = null!;

    /// <summary>
    /// Server service assignments associated with this service.
    /// </summary>
    public ICollection<ServerServiceEntity>
        ServerServices
    {
        get;
        set;
    } = new List<ServerServiceEntity>();
}