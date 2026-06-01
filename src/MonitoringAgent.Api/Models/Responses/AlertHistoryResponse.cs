public sealed class AlertHistoryResponse
{
    public long AlertEventId { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string Message { get; set; }
        = string.Empty;

    public DateTime OpenedUtc { get; set; }

    public DateTime? ClosedUtc { get; set; }

    public DateTime? FirstTriggeredUtc { get; set; }

    public int OccurrenceCount { get; set; }

    public string RuleName { get; set; }
        = string.Empty;
}