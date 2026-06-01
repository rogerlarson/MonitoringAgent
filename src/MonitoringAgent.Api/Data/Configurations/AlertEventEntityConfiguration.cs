using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class AlertEventEntityConfiguration
    : IEntityTypeConfiguration<AlertEventEntity>
{
    public void Configure(
        EntityTypeBuilder<AlertEventEntity> builder)
    {
        builder.ToTable("alert_events");

        builder.HasKey(x =>
            x.AlertEventId);

        builder.Property(x =>
            x.AlertEventId)
            .HasColumnName("alert_event_id");

        builder.Property(x =>
            x.AlertRuleId)
            .HasColumnName("alert_rule_id");

        builder.Property(x =>
            x.ServerId)
            .HasColumnName("server_id");

        builder.Property(x =>
            x.ServerServiceId)
            .HasColumnName("server_service_id");

        builder.Property(x =>
            x.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        builder.Property(x =>
            x.Message)
            .HasColumnName("message");

        builder.Property(x =>
            x.OpenedUtc)
            .HasColumnName("opened_utc");

        builder.Property(x =>
            x.AcknowledgedUtc)
            .HasColumnName("acknowledged_utc");

        builder.Property(x =>
            x.AcknowledgedBy)
            .HasColumnName("acknowledged_by");

        builder.Property(x =>
            x.SuppressedUntilUtc)
            .HasColumnName("suppressed_until_utc");

        builder.Property(x =>
            x.ClosedUtc)
            .HasColumnName("closed_utc");

        builder.Property(x =>
            x.LastSeenUtc)
            .HasColumnName("last_seen_utc");

        builder.Property(x =>
            x.OccurrenceCount)
            .HasColumnName("occurrence_count");

        builder.Property(x =>
            x.FirstTriggeredUtc)
            .HasColumnName(
                "first_triggered_utc");

        builder.Property(x =>
            x.LastNotificationUtc)
            .HasColumnName(
                "last_notification_utc");

        builder.Property(x =>
            x.NotificationCount)
            .HasColumnName(
                "notification_count");

        builder.Property(x =>
            x.SuppressedBy)
            .HasColumnName("suppressed_by");
        
        builder.Property(x =>
            x.SuppressedUtc)
            .HasColumnName("suppressed_utc");

        builder.Property(x =>
            x.ClosedBy)
            .HasColumnName("closed_by");

        builder.HasOne(x =>
            x.AlertRule)
            .WithMany()
            .HasForeignKey(x =>
                x.AlertRuleId);

        builder.HasOne(x =>
            x.Server)
            .WithMany()
            .HasForeignKey(x =>
                x.ServerId);

        builder.HasOne(x =>
            x.ServerService)
            .WithMany()
            .HasForeignKey(x =>
                x.ServerServiceId);
    }
}