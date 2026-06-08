// ============================================================================
// Project: MonitoringAgent
// File: IMonitoringRepository.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines data access operations required by the monitoring engine.
//
//      Provides an abstraction layer between monitoring services and the
//      underlying persistence implementation, allowing monitoring logic to
//      remain independent of Entity Framework, SQL Server, or other data
//      storage technologies.
// ============================================================================

using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;

namespace MonitoringAgent.Common.Interfaces;

/// <summary>
/// Provides monitoring data access operations required by the monitoring
/// engine.
/// </summary>
public interface IMonitoringRepository
{
    // =====================================================================
    // Server Management
    // =====================================================================

    /// <summary>
    /// Retrieves all monitored servers.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token for the operation.
    /// </param>
    /// <returns>
    /// Collection of monitored servers.
    /// </returns>
    Task<List<ServerEntity>> GetServersAsync(
        CancellationToken cancellationToken);

    // =====================================================================
    // Snapshot Retrieval
    // =====================================================================

    /// <summary>
    /// Retrieves the most recent health snapshot for a server.
    /// </summary>
    /// <param name="serverId">
    /// Identifier of the server.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token for the operation.
    /// </param>
    /// <returns>
    /// Most recent snapshot if available; otherwise null.
    /// </returns>
    Task<HostSnapshotEntity?> GetLatestSnapshotAsync(
        int serverId,
        CancellationToken cancellationToken);

    // =====================================================================
    // Status Updates
    // =====================================================================

    /// <summary>
    /// Updates the current monitoring status of a server.
    /// </summary>
    /// <param name="serverId">
    /// Identifier of the server.
    /// </param>
    /// <param name="status">
    /// New server status value.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token for the operation.
    /// </param>
    Task UpdateServerStatusAsync(
        int serverId,
        ServerStatus status,
        CancellationToken cancellationToken);
}