using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Service Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for monitoring service
/// definitions.
///
/// Services represent monitorable application types that can
/// be assigned to monitored servers.
///
/// Examples:
/// - Ignition Gateway
/// - SQL Server
/// - IIS Website
/// - MQTT Broker
/// - OPC-UA Server
///
/// Services provide the monitoring template while
/// ServerServiceEntity represents an actual deployment of
/// that service on a specific server.
///
/// Table:
///     services
///
/// Relationships:
///
/// ServiceType
///     ↓
/// Services
///
/// Service
///     ↓
/// ServerServices
/// ============================================================================
/// </summary>
public sealed class ServiceEntityConfiguration
    : IEntityTypeConfiguration<ServiceEntity>
{
    // -------------------------------------------------------------------------
    // Entity Configuration
    // -------------------------------------------------------------------------

    public void Configure(
        EntityTypeBuilder<ServiceEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable(
            "services");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(
            x => x.ServiceId);

        builder.Property(
            x => x.ServiceId)
            .HasColumnName(
                "service_id");

        // ---------------------------------------------------------------------
        // Service Definition
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServiceName)
            .HasColumnName(
                "service_name");

        builder.Property(
            x => x.ServiceTypeId)
            .HasColumnName(
                "service_type_id");

        // ---------------------------------------------------------------------
        // Collection Configuration
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.CollectorName)
            .HasColumnName(
                "collector_name");

        builder.Property(
            x => x.RegistrationMode)
            .HasColumnName(
                "registration_mode")
            .HasConversion<string>();

        // ---------------------------------------------------------------------
        // Audit Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.CreatedDateUtc)
            .HasColumnName(
                "created_date_utc");

        // ---------------------------------------------------------------------
        // Service Type Relationship
        // ---------------------------------------------------------------------

        builder.HasOne(
                x => x.ServiceType)
            .WithMany(
                x => x.Services)
            .HasForeignKey(
                x => x.ServiceTypeId);
    }
}