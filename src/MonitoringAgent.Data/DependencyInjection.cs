// ============================================================================
// Project: MonitoringAgent.Data
// File: DependencyInjection.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides dependency injection registration helpers for the
//      MonitoringAgent data layer.
//
//      Registers database services, Entity Framework Core infrastructure,
//      and persistence-related components.
// ============================================================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MonitoringAgent.Data;

/// <summary>
/// Dependency injection registration helpers for the data layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers MonitoringAgent data services.
    /// </summary>
    /// <param name="services">
    /// Service collection being configured.
    /// </param>
    /// <param name="configuration">
    /// Application configuration source.
    /// </param>
    /// <returns>
    /// Updated service collection.
    /// </returns>
    public static IServiceCollection
        AddMonitoringData(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        return null!;
    }
}