public sealed class ServerSummaryResponse
{
    public int ServerId
    { get; set; }

    public string ServerName
    { get; set; }
        = string.Empty;

    public bool Online
    { get; set; }

    public DateTime? LastSeenUtc
    { get; set; }

    public int OpenAlertCount
    { get; set; }
}