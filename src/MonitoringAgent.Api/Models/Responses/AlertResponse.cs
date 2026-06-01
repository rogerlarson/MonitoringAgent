namespace MonitoringAgent.Api.Models.Responses;

public sealed class AlertResponse
{
    public long AlertEventId
    { get; set; }

    public int AlertRuleId
    { get; set; }

    public int ServerId
    { get; set; }

    public int? ServerServiceId
    { get; set; }

    public string Status
    { get; set; }
        = string.Empty;

    public string Message
    { get; set; }
        = string.Empty;

    public DateTime OpenedUtc
    { get; set; }

    public DateTime? ClosedUtc
    { get; set; }

    public DateTime? LastSeenUtc
    { get; set; }

    public DateTime? FirstTriggeredUtc
    { get; set; }

    public DateTime? LastNotificationUtc
    { get; set; }

    public int OccurrenceCount
    { get; set; }

    public DateTime? AcknowledgedUtc
    { get; set; }

    public string? AcknowledgedBy
    { get; set; }

    public DateTime? SuppressedUntilUtc
    { get; set; }

    public int NotificationCount
    { get; set; }

    public string? SuppressedBy
    { get; set; }

    public DateTime? SuppressedUtc
    { get; set; }

    public string? ClosedBy
    { get; set; }
}