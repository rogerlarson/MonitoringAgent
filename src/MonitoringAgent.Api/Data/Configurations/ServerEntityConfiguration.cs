using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class ServerEntityConfiguration
    : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(
        EntityTypeBuilder<ServerEntity> builder)
    {
        builder.ToTable("servers");

        builder.HasKey(x => x.ServerId);

        builder.Property(x => x.ServerId)
            .HasColumnName("server_id");

        builder.Property(x => x.ServerName)
            .HasColumnName("server_name");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(50);

        builder.Property(x => x.OperatingSystem)
            .HasColumnName("operating_system")
            .HasMaxLength(200);

        builder.Property(x => x.OperatingSystemVersion)
            .HasColumnName("operating_system_version")
            .HasMaxLength(500);

        builder.Property(x => x.ProcessorCount)
            .HasColumnName("processor_count");

        builder.Property(x => x.TotalMemoryMb)
            .HasColumnName("total_memory_mb");
       
        builder.Property(x => x.AgentVersion)
            .HasColumnName("agent_version")
            .HasMaxLength(255);

        builder.Property(x => x.DomainName)
            .HasColumnName("domain_name")
            .HasMaxLength(255);

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.Property(x => x.LastSeenUtc)
            .HasColumnName("last_seen_utc");
    }
}