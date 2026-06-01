namespace MonitoringAgent.Api.Models.Responses;

public sealed class GatewayStatusResponse
{
    public int ServerServiceId
    { get; set; }

    public string ServiceName
    { get; set; }
        = string.Empty;

    public DateTime SnapshotUtc
    { get; set; }

    public string Status
    { get; set; }
        = "Unknown";

    public bool Reachable
    { get; set; }

    public int HttpStatusCode
    { get; set; }

    public long ResponseMs
    { get; set; }
}