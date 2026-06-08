using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Server Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for monitored servers.
///
/// Servers represent monitored hosts within the monitoring
/// platform and serve as the parent entity for collected
/// monitoring snapshots.
///
/// Related Data:
/// - Host Snapshots
/// - Gateway Snapshots
/// - Ignition Snapshots
/// - Alert Events
///
/// Table:
///     servers
/// ============================================================================
/// </summary>
public sealed class ServerEntityConfiguration
    : IEntityTypeConfiguration<ServerEntity>
{
    // -------------------------------------------------------------------------
    // Entity Configuration
    // -------------------------------------------------------------------------

    public void Configure(
        EntityTypeBuilder<ServerEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable(
            "servers");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(
            x => x.ServerId);

        builder.Property(
            x => x.ServerId)
            .HasColumnName(
                "server_id");

        // ---------------------------------------------------------------------
        // Server Identity
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ServerName)
            .HasColumnName(
                "server_name");

        builder.Property(
            x => x.DomainName)
            .HasColumnName(
                "domain_name")
            .HasMaxLength(255);

        // ---------------------------------------------------------------------
        // Current Status
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.Status)
            .HasConversion<string>()
            .HasColumnName(
                "status")
            .HasMaxLength(50);

        builder.Property(
            x => x.LastSeenUtc)
            .HasColumnName(
                "last_seen_utc");

        // ---------------------------------------------------------------------
        // Operating System Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.OperatingSystem)
            .HasColumnName(
                "operating_system")
            .HasMaxLength(200);

        builder.Property(
            x => x.OperatingSystemVersion)
            .HasColumnName(
                "operating_system_version")
            .HasMaxLength(500);

        // ---------------------------------------------------------------------
        // Hardware Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.ProcessorCount)
            .HasColumnName(
                "processor_count");

        builder.Property(
            x => x.TotalMemoryMb)
            .HasColumnName(
                "total_memory_mb");

        // ---------------------------------------------------------------------
        // Agent Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.AgentVersion)
            .HasColumnName(
                "agent_version")
            .HasMaxLength(255);

        // ---------------------------------------------------------------------
        // Audit Information
        // ---------------------------------------------------------------------

        builder.Property(
            x => x.CreatedDateUtc)
            .HasColumnName(
                "created_date_utc");
    }
}