using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class AlertRuleEntityConfiguration
    : IEntityTypeConfiguration<AlertRuleEntity>
{
    public void Configure(
        EntityTypeBuilder<AlertRuleEntity> builder)
    {
        builder.ToTable("alert_rules");

        builder.HasKey(x =>
            x.AlertRuleId);

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
            x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasMany(x =>
            x.AlertEvents)
            .WithOne(x =>
                x.AlertRule)
            .HasForeignKey(x =>
                x.AlertRuleId);
    }
}