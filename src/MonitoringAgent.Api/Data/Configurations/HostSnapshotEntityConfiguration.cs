using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class HostSnapshotEntityConfiguration
    : IEntityTypeConfiguration<HostSnapshotEntity>
{
    public void Configure(
        EntityTypeBuilder<HostSnapshotEntity> builder)
    {
        builder.ToTable("host_snapshots");

        builder.HasKey(x => x.SnapshotId);

        builder.Property(x => x.SnapshotId)
            .HasColumnName("snapshot_id");

        builder.Property(x => x.ServerId)
            .HasColumnName("server_id");

        builder.Property(x => x.SnapshotUtc)
            .HasColumnName("snapshot_utc");

        // System

        builder.Property(x => x.CpuPercent)
            .HasColumnName("cpu_percent")
            .HasPrecision(9, 2);

        builder.Property(x => x.MemoryPercent)
            .HasColumnName("memory_percent")
            .HasPrecision(9, 2);

        builder.Property(x => x.AvailableMemoryMb)
            .HasColumnName("available_memory_mb");

        builder.Property(x => x.ProcessCount)
            .HasColumnName("process_count");

        builder.Property(x => x.SystemUptimeMinutes)
            .HasColumnName("system_uptime_minutes");

        builder.Property(x => x.ContextSwitchesPerSec)
            .HasColumnName("context_switches_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.PageFaultsPerSec)
            .HasColumnName("page_faults_per_sec")
            .HasPrecision(18, 2);

        // Disk

        builder.Property(x => x.SystemDrive)
            .HasColumnName("system_drive");

        builder.Property(x => x.DiskPercentUsed)
            .HasColumnName("disk_percent_used")
            .HasPrecision(9, 2);

        builder.Property(x => x.DiskFreeGb)
            .HasColumnName("disk_free_gb")
            .HasPrecision(18, 2);

        builder.Property(x => x.DiskReadsPerSec)
            .HasColumnName("disk_reads_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.DiskWritesPerSec)
            .HasColumnName("disk_writes_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.DiskReadLatencyMs)
            .HasColumnName("disk_read_latency_ms")
            .HasPrecision(18, 2);

        builder.Property(x => x.DiskWriteLatencyMs)
            .HasColumnName("disk_write_latency_ms")
            .HasPrecision(18, 2);

        builder.Property(x => x.DiskQueueLength)
            .HasColumnName("disk_queue_length")
            .HasPrecision(18, 2);

        builder.Property(x => x.AvgDiskQueueLength)
            .HasColumnName("avg_disk_queue_length")
            .HasPrecision(18, 2);

        // Network

        builder.Property(x => x.PrimaryNetworkInterface)
            .HasColumnName("primary_network_interface");

        builder.Property(x => x.NetworkBytesReceivedPerSec)
            .HasColumnName("network_bytes_received_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.NetworkBytesSentPerSec)
            .HasColumnName("network_bytes_sent_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.NetworkReceiveErrors)
            .HasColumnName("network_receive_errors");

        builder.Property(x => x.NetworkSendErrors)
            .HasColumnName("network_send_errors");

        builder.Property(x => x.NetworkReceiveDiscards)
            .HasColumnName("network_receive_discards");

        builder.Property(x => x.NetworkSendDiscards)
            .HasColumnName("network_send_discards");

        builder.Property(x => x.TcpRetransmissionsPerSec)
            .HasColumnName("tcp_retransmissions_per_sec")
            .HasPrecision(18, 2);

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasOne(x => x.Server)
            .WithMany(x => x.HealthSnapshots)
            .HasForeignKey(x => x.ServerId);
    }
}