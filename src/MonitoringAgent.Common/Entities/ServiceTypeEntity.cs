// ============================================================================
// Project: MonitoringAgent
// File: ServiceTypeEntity.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Represents a category of monitorable services within the monitoring
//      platform.
//
//      Service types are used to organize service definitions into logical
//      groups such as Ignition, Gateway, SQL Server, IIS, or other service
//      categories supported by the monitoring engine.
// ============================================================================

namespace MonitoringAgent.Common.Entities;

/// <summary>
/// Represents a category of monitorable services.
/// </summary>
public sealed class ServiceTypeEntity
{
    // =====================================================================
    // Identity
    // =====================================================================

    /// <summary>
    /// Unique identifier for the service type.
    /// </summary>
    public int ServiceTypeId
    {
        get;
        set;
    }

    /// <summary>
    /// Display name of the service type.
    /// </summary>
    public string ServiceTypeName
    {
        get;
        set;
    } = string.Empty;

    // =====================================================================
    // Configuration
    // =====================================================================

    /// <summary>
    /// Optional description of the service type.
    /// </summary>
    public string? Description
    {
        get;
        set;
    }

    // =====================================================================
    // Audit Information
    // =====================================================================

    /// <summary>
    /// UTC timestamp when the service type was created.
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
    /// Service definitions associated with this service type.
    /// </summary>
    public ICollection<ServiceEntity>
        Services
    {
        get;
        set;
    } = new List<ServiceEntity>();
}