using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Server Service Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for monitored services
/// assigned to servers.
///
/// Server services define which monitoring targets exist on
/// a particular server.
///
/// Examples:
/// - Ignition Gateway
/// - Ignition Service
/// - Web Application
/// - SQL Server
///
/// Server services act as the parent entity for:
/// - Gateway Snapshots
/// - Ignition Snapshots
///
/// Table:
///     server_services
///
/// Relationships:
///
/// Server
///     ↓
/// ServerServices
///
/// Service
///     ↓
/// ServerServices
/// ============================================================================
/// </summary>
public sealed class ServerServiceEntityConfiguration
    : IEntityTypeConfiguration<ServerServiceEntity>
{
    // -------------------------------------------------------------------------
    // Entity Configuration
    // -------------------------------------------------------------------------

    public void Configure(
        EntityTypeBuilder<ServerServiceEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable(
            "server_services");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(
            x => x.ServerServiceId);

        builder.Property(
            x => x.ServerServiceId)
            .HasColumnName(
                "server_service_id");

        // ---------------------------------------------------------------------
        // Relationships
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServerId)
            .HasColumnName(
                "server_id");

        builder.Property(
            x => x.ServiceId)
            .HasColumnName(
                "service_id");

        // ---------------------------------------------------------------------
        // Service Configuration
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServiceInstanceName)
            .HasColumnName(
                "service_instance_name");

        builder.Property(
            x => x.Enabled)
            .HasColumnName(
                "is_enabled");

        // ---------------------------------------------------------------------
        // Audit Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.CreatedDateUtc)
            .HasColumnName(
                "created_date_utc");

        // ---------------------------------------------------------------------
        // Server Relationship
        // ---------------------------------------------------------------------

        builder.HasOne(
                x => x.Server)
            .WithMany(
                x => x.ServerServices)
            .HasForeignKey(
                x => x.ServerId);

        // ---------------------------------------------------------------------
        // Service Relationship
        // ---------------------------------------------------------------------

        builder.HasOne(
                x => x.Service)
            .WithMany(
                x => x.ServerServices)
            .HasForeignKey(
                x => x.ServiceId);
    }
}