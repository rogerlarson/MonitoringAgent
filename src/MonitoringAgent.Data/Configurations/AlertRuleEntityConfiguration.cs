/*
===============================================================================
AlertRuleEntityConfiguration
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Configures Entity Framework Core mappings for AlertRuleEntity.

Responsibilities:
- Configure table mapping
- Configure column mappings
- Configure enum persistence
- Configure precision rules
- Configure alert rule relationships

Database Table:
    alert_rules

Relationships:

    AlertRule
        ↓
    AlertEvents

Notes:
Alert rules define monitoring behavior and determine when
alerts should be created, repeated, and automatically closed.

Examples:

    CPU > 95%
    Memory > 90%
    Disk > 95%
    Gateway Reachable = False

Alert rules are evaluated by:

    SnapshotAlertWorker
            ↓
    AlertService

===============================================================================
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Common.Entities;

namespace MonitoringAgent.Data.Configurations;

/// <summary>
/// ============================================================================
/// Alert Rule Entity Configuration
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Configures Entity Framework mappings for alert rule definitions.
///
/// Alert rules define:
/// - Metrics to evaluate
/// - Threshold values
/// - Comparison operators
/// - Severity levels
/// - Notification behavior
/// - Auto-close behavior
///
/// Alert rules are evaluated by:
/// - SnapshotAlertWorker
/// - AlertService
///
/// Table:
///     alert_rules
/// ============================================================================
public sealed class AlertRuleEntityConfiguration
    : IEntityTypeConfiguration<AlertRuleEntity>
{
    public void Configure(
        EntityTypeBuilder<AlertRuleEntity> builder)
    {
        // ---------------------------------------------------------------------
        // Table
        // ---------------------------------------------------------------------

        builder.ToTable("alert_rules");

        // ---------------------------------------------------------------------
        // Primary Key
        // ---------------------------------------------------------------------

        builder.HasKey(x =>
            x.AlertRuleId);

        // ---------------------------------------------------------------------
        // Rule Definition
        // ---------------------------------------------------------------------

        builder.Property(x =>
            x.AlertRuleId)
            .HasColumnName("alert_rule_id");

        builder.Property(x =>
            x.RuleName)
            .HasColumnName("rule_name");

        builder.Property(x =>
            x.MetricName)
            .HasColumnName("metric_name");

        builder.Property(x => x.Operator)
            .HasColumnName("operator")
            .HasConversion<string>();

        builder.Property(x =>
            x.ThresholdValue)
            .HasColumnName("threshold_value")
            .HasPrecision(18, 2);

        builder.Property(x =>
            x.Severity)
            .HasConversion<string>()
            .HasColumnName(
                "severity");

        // ---------------------------------------------------------------------
        // Rule Behavior
        // ---------------------------------------------------------------------

        builder.Property(x =>
            x.Enabled)
            .HasColumnName("is_enabled");

        builder.Property(x =>
            x.SustainSeconds)
            .HasColumnName("sustain_seconds");

        builder.Property(x =>
            x.RepeatSeconds)
            .HasColumnName("repeat_seconds");

        builder.Property(x =>
            x.AutoClose)
            .HasColumnName("auto_close");

        builder.Property(x =>
            x.EmailEnabled)
            .HasColumnName("email_enabled");

        // ---------------------------------------------------------------------
        // Audit Information
        // ---------------------------------------------------------------------

        builder.Property(x =>
            x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        // ---------------------------------------------------------------------
        // Relationships
        // ---------------------------------------------------------------------

        builder.HasMany(x =>
            x.AlertEvents)
            .WithOne(x =>
                x.AlertRule)
            .HasForeignKey(x =>
                x.AlertRuleId);
    }
}