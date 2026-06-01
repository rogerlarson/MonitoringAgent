using MonitoringAgent.Api.Data.Enums;

namespace MonitoringAgent.Api.Data.Entities;

public sealed class AlertEventEntity
{
    public long AlertEventId
    { get; set; }

    public int AlertRuleId
    { get; set; }

    public int ServerId
    { get; set; }

    public int? ServerServiceId
    { get; set; }

    public AlertStatus Status
    { get; set; }

    public string Message
    { get; set; }
        = string.Empty;

    public DateTime OpenedUtc
    { get; set; }

    public DateTime? AcknowledgedUtc
    { get; set; }

    public string? AcknowledgedBy
    { get; set; }

    public DateTime? SuppressedUntilUtc
    { get; set; }

    public DateTime? ClosedUtc
    { get; set; }

    public DateTime? LastSeenUtc
    { get; set; }

    public int OccurrenceCount
    { get; set; }

    public AlertRuleEntity AlertRule
    { get; set; }
        = null!;

    public ServerEntity Server
    { get; set; }
        = null!;

    public ServerServiceEntity? ServerService
    { get; set; }

    public DateTime? FirstTriggeredUtc
    { get; set; }

    public DateTime? LastNotificationUtc
    { get; set; }

    public int NotificationCount
    { get; set; }
}