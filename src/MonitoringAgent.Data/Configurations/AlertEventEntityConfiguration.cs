/*
===============================================================================
AlertEventEntityConfiguration
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Configures Entity Framework Core mappings for AlertEventEntity.

Responsibilities:
- Configure table mapping
- Configure column mappings
- Configure enum conversions
- Configure foreign key relationships
- Define persistence behavior

Database Table:
    alert_events

Relationships:
    AlertEvent
        ├── AlertRule
        ├── Server
        └── ServerService

Notes:
This table represents the alert lifecycle and serves as the
primary operational record for alert management.

Alert events may transition through multiple states:

    Open
        ↓
    Acknowledged
        ↓
    Suppressed
        ↓
    Closed

The table also tracks:

- Alert occurrences
- Notification history
- Acknowledgement information
- Suppression information
- Closure information

===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Alert Event Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for alert events.
///
/// Alert events represent the operational lifecycle of alerts.
///
/// Lifecycle:
///
/// Open
///     ↓
/// Acknowledged
///     ↓
/// Suppressed
///     ↓
/// Closed
///
/// Table:
///     alert_events
/// ============================================================================
public sealed class AlertEventEntityConfiguration
    : IEntityTypeConfiguration<AlertEventEntity>
{
    public void Configure(
        EntityTypeBuilder<AlertEventEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable("alert_events");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(x =>
            x.AlertEventId);

        // ---------------------------------------------------------------------
        // Core Alert Information
        // ---------------------------------------------------------------------

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

        // ---------------------------------------------------------------------
        // Lifecycle Timestamps
        // ---------------------------------------------------------------------

        builder.Property(x =>
            x.OpenedUtc)
            .HasColumnName("opened_utc");

        builder.Property(x =>
            x.AcknowledgedUtc)
            .HasColumnName("acknowledged_utc");

        builder.Property(x =>
            x.SuppressedUtc)
            .HasColumnName("suppressed_utc");

        builder.Property(x =>
            x.SuppressedUntilUtc)
            .HasColumnName("suppressed_until_utc");

        builder.Property(x =>
            x.ClosedUtc)
            .HasColumnName("closed_utc");

        builder.Property(x =>
            x.LastSeenUtc)
            .HasColumnName("last_seen_utc");

        // ---------------------------------------------------------------------
        // Operator Actions
        // ---------------------------------------------------------------------

        builder.Property(x =>
            x.AcknowledgedBy)
            .HasColumnName("acknowledged_by");

        builder.Property(x =>
            x.SuppressedBy)
            .HasColumnName("suppressed_by");

        builder.Property(x =>
            x.ClosedBy)
            .HasColumnName("closed_by");

        // ---------------------------------------------------------------------
        // Alert Tracking
        // ---------------------------------------------------------------------

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

        // ---------------------------------------------------------------------
        // Relationships
        // ---------------------------------------------------------------------

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