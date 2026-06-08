// ============================================================================
// Project: MonitoringAgent.Agent
// File: IHealthPoster.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines the contract used to publish health snapshots collected by
//      the monitoring agent.
//
//      Implementations are responsible for transmitting snapshot data to
//      the central monitoring API or other configured destinations.
// ============================================================================

using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Services.Interfaces;

/// <summary>
/// Publishes health snapshots collected by the monitoring agent.
/// </summary>
public interface IHealthPoster
{
    // =====================================================================
    // Snapshot Publishing
    // =====================================================================

    /// <summary>
    /// Publishes a health snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// Health snapshot to publish.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token for the operation.
    /// </param>
    Task PublishAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken);
}