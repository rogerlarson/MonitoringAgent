namespace MonitoringAgent.Api.Models.Responses;

public sealed class GatewayHistoryResponse
{
    public DateTime SnapshotUtc
    { get; set; }

    public bool Reachable
    { get; set; }

    public int HttpStatusCode
    { get; set; }

    public long ResponseMs
    { get; set; }
}