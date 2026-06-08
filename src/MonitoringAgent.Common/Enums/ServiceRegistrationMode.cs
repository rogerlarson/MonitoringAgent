// ============================================================================
// Project: MonitoringAgent
// File: ServiceRegistrationMode.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines how a monitored service becomes registered within the
//      monitoring platform.
//
//      Registration modes determine whether a service is manually created,
//      globally applied to all servers, or automatically discovered during
//      service detection processes.
// ============================================================================

namespace MonitoringAgent.Common.Enums;

/// <summary>
/// Defines the registration method used for a monitored service.
/// </summary>
public enum ServiceRegistrationMode
{
    /// <summary>
    /// Service registration is created manually by an administrator.
    /// </summary>
    Manual = 1,

    /// <summary>
    /// Service registration is applied globally to all applicable servers.
    /// </summary>
    Global = 2,

    /// <summary>
    /// Service registration is automatically created through service
    /// discovery or detection.
    /// </summary>
    Detected = 3
}