using MonitoringAgent.Api.Data.Enums;

namespace MonitoringAgent.Api.Data.Entities;

public sealed class AlertRuleEntity
{
    public int AlertRuleId
    { get; set; }

    public string RuleName
    { get; set; }
        = string.Empty;

    public string MetricName
    { get; set; }
        = string.Empty;

    public AlertOperator Operator
    { get; set; }

    public decimal? ThresholdValue
    { get; set; }

    public AlertSeverity Severity
    { get; set; }

    public bool Enabled
    { get; set; }

    public int SustainSeconds
    { get; set; }

    public int RepeatSeconds
    { get; set; }

    public bool AutoClose
    { get; set; }

    public DateTime CreatedDateUtc
    { get; set; }

    public ICollection<AlertEventEntity>
        AlertEvents
    { get; set; }
        = new List<AlertEventEntity>();
}