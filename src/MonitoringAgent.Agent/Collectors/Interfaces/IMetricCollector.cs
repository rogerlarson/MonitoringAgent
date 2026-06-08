// ============================================================================
// Project: MonitoringAgent.Agent
// File: IMetricCollector.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines the contract used to collect health metrics from the local
//      machine.
//
//      Implementations are responsible for gathering system, application,
//      and infrastructure metrics and returning a populated health snapshot.
// ============================================================================

using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors.Interfaces;

/// <summary>
/// Defines health metric collection operations.
/// </summary>
public interface IMetricCollector
{
    // =====================================================================
    // Metric Collection
    // =====================================================================

    /// <summary>
    /// Collects all available health metrics and returns a populated
    /// health snapshot.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// A populated health snapshot.
    /// </returns>
    Task<HealthSnapshot> CollectAsync(
        CancellationToken cancellationToken);
}