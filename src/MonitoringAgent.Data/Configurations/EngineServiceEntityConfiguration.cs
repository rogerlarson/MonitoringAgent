/*
===============================================================================
EngineServiceEntityConfiguration
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Configures Entity Framework Core mappings for EngineServiceEntity.

Responsibilities:
- Configure table mapping
- Configure worker telemetry persistence
- Configure execution statistics storage
- Configure lifecycle tracking storage

Database Table:
    engine_services

Tracked Information:
- Service name
- Runtime status
- Startup time
- Last execution
- Last success
- Run count
- Error count
- Execution duration
- Last error
- Instance identifier

Used By:
- EngineStatusService
- HostOfflineMonitorWorker
- SnapshotAlertWorker
- SnapshotCleanupWorker
- LogCleanupWorker

Notes:
This table provides operational visibility into
background worker activity and is the primary
data source for the Engine Status page.

Each worker maintains a single record that is
continuously updated throughout its lifecycle.

===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Engine Service Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for engine worker status tracking.
///
/// This table stores runtime telemetry for background services.
///
/// Examples:
/// - SnapshotAlertWorker
/// - HostOfflineMonitorWorker
/// - SnapshotCleanupWorker
/// - LogCleanupWorker
///
/// Table:
///     engine_services
/// ============================================================================
internal sealed class EngineServiceEntityConfiguration
    : IEntityTypeConfiguration<EngineServiceEntity>
{
    public void Configure(
        EntityTypeBuilder<EngineServiceEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable(
            "engine_services");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(
            x => x.EngineServiceId);

        builder.Property(
            x => x.EngineServiceId)
            .HasColumnName(
                "engine_service_id");

        // ---------------------------------------------------------------------
        // Service Identity
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServiceName)
            .HasColumnName(
                "service_name");

        builder.Property(
            x => x.InstanceId)
            .HasColumnName(
                "instance_id");

        // ---------------------------------------------------------------------
        // Runtime Status
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.Status)
            .HasColumnName(
                "status");

        // ---------------------------------------------------------------------
        // Lifecycle Tracking
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.StartedUtc)
            .HasColumnName(
                "started_utc");

        builder.Property(
            x => x.LastRunUtc)
            .HasColumnName(
                "last_run_utc");

        builder.Property(
            x => x.LastSuccessUtc)
            .HasColumnName(
                "last_success_utc");

        // ---------------------------------------------------------------------
        // Execution Statistics
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.RunCount)
            .HasColumnName(
                "run_count");

        builder.Property(
            x => x.ErrorCount)
            .HasColumnName(
                "error_count");

        builder.Property(
            x => x.LastDurationMs)
            .HasColumnName(
                "last_duration_ms");

        // ---------------------------------------------------------------------
        // Error Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.LastError)
            .HasColumnName(
                "last_error");
    }
}