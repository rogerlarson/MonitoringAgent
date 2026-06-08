using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Service Type Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for monitoring service types.
///
/// Service types represent the highest-level classification
/// of monitorable applications and platforms.
///
/// Examples:
/// - Ignition
/// - Database
/// - Web Application
/// - Messaging
/// - Infrastructure
///
/// Service types provide organizational structure for
/// monitoring services and allow services to be grouped
/// by technology or purpose.
///
/// Hierarchy:
///
/// ServiceType
///     ↓
/// Service
///     ↓
/// ServerService
///
/// Table:
///     service_types
///
/// Relationships:
///
/// ServiceType
///     ↓
/// Services
/// ============================================================================
/// </summary>
public sealed class ServiceTypeEntityConfiguration
    : IEntityTypeConfiguration<ServiceTypeEntity>
{
    // -------------------------------------------------------------------------
    // Entity Configuration
    // -------------------------------------------------------------------------

    public void Configure(
        EntityTypeBuilder<ServiceTypeEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable(
            "service_types");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(
            x => x.ServiceTypeId);

        builder.Property(
            x => x.ServiceTypeId)
            .HasColumnName(
                "service_type_id");

        // ---------------------------------------------------------------------
        // Service Type Definition
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServiceTypeName)
            .HasColumnName(
                "service_type_name");

        builder.Property(
            x => x.Description)
            .HasColumnName(
                "description");

        // ---------------------------------------------------------------------
        // Audit Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.CreatedDateUtc)
            .HasColumnName(
                "created_date_utc");
    }
}