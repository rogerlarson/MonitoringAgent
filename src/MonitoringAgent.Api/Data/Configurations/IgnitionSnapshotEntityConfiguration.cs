using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class IgnitionSnapshotEntityConfiguration
    : IEntityTypeConfiguration<IgnitionSnapshotEntity>
{
    public void Configure(
        EntityTypeBuilder<IgnitionSnapshotEntity> builder)
    {
        builder.ToTable("ignition_snapshots");

        builder.HasKey(
            x => x.IgnitionSnapshotId);

        builder.Property(x => x.IgnitionSnapshotId)
            .HasColumnName("ignition_snapshot_id");

        builder.Property(x => x.ServerServiceId)
            .HasColumnName("server_service_id");

        builder.Property(x => x.SnapshotUtc)
            .HasColumnName("snapshot_utc");

        builder.Property(x => x.ServiceRunning)
            .HasColumnName("service_running");

        builder.Property(x => x.ProcessRunning)
            .HasColumnName("process_running");

        builder.Property(x => x.IgnitionVersion)
            .HasColumnName("ignition_version");

        builder.Property(x => x.JavaVersion)
            .HasColumnName("java_version");

        builder.Property(x => x.CpuPercent)
            .HasColumnName("cpu_percent")
            .HasPrecision(9, 2);

        builder.Property(x => x.MemoryMb)
            .HasColumnName("memory_mb");

        builder.Property(x => x.ThreadCount)
            .HasColumnName("thread_count");

        builder.Property(x => x.HandleCount)
            .HasColumnName("handle_count");

        builder.Property(x => x.UptimeMinutes)
            .HasColumnName("uptime_minutes");

        builder.Property(x => x.ProcessId)
            .HasColumnName("process_id");

        builder.Property(x => x.ProcessName)
            .HasColumnName("process_name");

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasOne(x => x.ServerService)
            .WithMany(x => x.IgnitionSnapshots)
            .HasForeignKey(x => x.ServerServiceId);
    }
}