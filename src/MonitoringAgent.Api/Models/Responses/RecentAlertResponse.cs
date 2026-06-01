namespace MonitoringAgent.Api.Models.Responses;

public sealed class RecentAlertResponse
{
    public long AlertEventId
    { get; set; }

    public int ServerId
    { get; set; }

    public string RuleName
    { get; set; }
        = string.Empty;

    public string Severity
    { get; set; }
        = string.Empty;

    public string Status
    { get; set; }
        = string.Empty;

    public string Message
    { get; set; }
        = string.Empty;

    public DateTime OpenedUtc
    { get; set; }

    public DateTime? LastSeenUtc
    { get; set; }

    public int OccurrenceCount
    { get; set; }

    public int NotificationCount
    { get; set; }
}