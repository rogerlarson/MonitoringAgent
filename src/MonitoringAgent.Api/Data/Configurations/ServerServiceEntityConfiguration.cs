using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class ServerServiceEntityConfiguration
    : IEntityTypeConfiguration<ServerServiceEntity>
{
    public void Configure(
        EntityTypeBuilder<ServerServiceEntity> builder)
    {
        builder.ToTable("server_services");

        builder.HasKey(
            x => x.ServerServiceId);

        builder.Property(x => x.ServerServiceId)
            .HasColumnName("server_service_id");

        builder.Property(x => x.ServerId)
            .HasColumnName("server_id");

        builder.Property(x => x.ServiceId)
            .HasColumnName("service_id");

        builder.Property(x => x.ServiceInstanceName)
            .HasColumnName("service_instance_name");

        builder.Property(x => x.Enabled)
            .HasColumnName("is_enabled");

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasOne(x => x.Server)
            .WithMany(x => x.ServerServices)
            .HasForeignKey(x => x.ServerId);

        builder.HasOne(x => x.Service)
            .WithMany(x => x.ServerServices)
            .HasForeignKey(x => x.ServiceId);
    }
}