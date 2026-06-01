namespace MonitoringAgent.Api.Data.Entities;

public sealed class GatewaySnapshotEntity
{
    public long GatewaySnapshotId
    { get; set; }

    public int ServerServiceId
    { get; set; }

    public DateTime SnapshotUtc
    { get; set; }

    public bool Reachable
    { get; set; }

    public int HttpStatusCode
    { get; set; }

    public long ResponseMs
    { get; set; }

    public DateTime CreatedDateUtc
    { get; set; }

    public ServerServiceEntity ServerService
    { get; set; }
        = null!;
}