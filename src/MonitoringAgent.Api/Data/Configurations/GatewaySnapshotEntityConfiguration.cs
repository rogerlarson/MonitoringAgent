using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class GatewaySnapshotEntityConfiguration
    : IEntityTypeConfiguration<GatewaySnapshotEntity>
{
    public void Configure(
        EntityTypeBuilder<GatewaySnapshotEntity> builder)
    {
        builder.ToTable("gateway_snapshots");

        builder.HasKey(
            x => x.GatewaySnapshotId);

        builder.Property(x => x.GatewaySnapshotId)
            .HasColumnName("gateway_snapshot_id");

        builder.Property(x => x.ServerServiceId)
            .HasColumnName("server_service_id");

        builder.Property(x => x.SnapshotUtc)
            .HasColumnName("snapshot_utc");

        builder.Property(x => x.Reachable)
            .HasColumnName("reachable");

        builder.Property(x => x.HttpStatusCode)
            .HasColumnName("http_status_code");

        builder.Property(x => x.ResponseMs)
            .HasColumnName("response_ms");

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasOne(x => x.ServerService)
            .WithMany(x => x.GatewaySnapshots)
            .HasForeignKey(x => x.ServerServiceId);
    }
}