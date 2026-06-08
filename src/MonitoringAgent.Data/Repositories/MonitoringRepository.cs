/*
===============================================================================
MonitoringRepository
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Provides data access operations for monitored servers and
host snapshot information.

Responsibilities:
- Retrieve monitored servers
- Retrieve latest host snapshots
- Update server status
- Encapsulate Entity Framework queries
- Provide persistence operations for monitoring services

Implements:
- IMonitoringRepository

Database Access:
- Servers
- HostSnapshots

Notes:
This repository serves as an abstraction layer between
business logic and Entity Framework persistence.

Current usage is focused on server monitoring operations,
but may expand to include additional monitoring-related
queries as the platform evolves.

===============================================================================
*/

// -------------------------------------------------------------------------
// Dependencies
// -------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Data;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Data.Repositories;

/// <summary>
/// Entity Framework implementation of
/// monitoring repository operations.
///
/// This class is responsible for all
/// server and snapshot persistence
/// required by the monitoring engine.
/// </summary>
public sealed class MonitoringRepository
    : IMonitoringRepository
{
    private readonly MonitoringDbContext _db;

    /// <summary>
    /// Creates a new repository instance.
    /// </summary>
    public MonitoringRepository(
        MonitoringDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves all monitored servers.
    /// </summary>
    // -------------------------------------------------------------------------
    // Server Retrieval
    // -------------------------------------------------------------------------
    public async Task<List<ServerEntity>>
        GetServersAsync(
            CancellationToken cancellationToken)
    {
        return await _db.Servers
            .ToListAsync(
                cancellationToken);
    }

    /// <summary>
    /// Retrieves the most recent host snapshot
    /// for the specified server.
    /// </summary>
    // -------------------------------------------------------------------------
    // Latest Host Snapshot Retrieval
    // -------------------------------------------------------------------------
    public async Task<HostSnapshotEntity?>
        GetLatestSnapshotAsync(
            int serverId,
            CancellationToken cancellationToken)
    {
        return await _db.HostSnapshots
            .Where(x =>
                x.ServerId ==
                serverId)
            .OrderByDescending(x =>
                x.SnapshotUtc)
            .FirstOrDefaultAsync(
                cancellationToken);
    }

    /// <summary>
    /// Updates the current calculated status
    /// of a monitored server.
    /// </summary>
    // -------------------------------------------------------------------------
    // Server Status Update
    // -------------------------------------------------------------------------
    public async Task UpdateServerStatusAsync(
        int serverId,
        ServerStatus status,
        CancellationToken cancellationToken)
    {
        var server =
            await _db.Servers
                .FirstOrDefaultAsync(
                    x => x.ServerId == serverId,
                    cancellationToken);

        // ---------------------------------------------------------------------
        // Server Not Found
        // ---------------------------------------------------------------------
        if (server == null)
        {
            return;
        }
        // ---------------------------------------------------------------------
        // Persist Status Change
        // ---------------------------------------------------------------------
        server.Status = status;

        await _db.SaveChangesAsync(
            cancellationToken);
    }
}