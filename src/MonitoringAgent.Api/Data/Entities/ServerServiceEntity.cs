namespace MonitoringAgent.Api.Data.Entities;

public sealed class ServerServiceEntity
{
    public int ServerServiceId { get; set; }

    public int ServerId { get; set; }

    public int ServiceId { get; set; }

    public string? ServiceInstanceName
    { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreatedDateUtc
    { get; set; }

    public ServerEntity Server
    { get; set; }
        = null!;

    public ServiceEntity Service
    { get; set; }
        = null!;

    public ICollection<IgnitionSnapshotEntity>
    IgnitionSnapshots
    { get; set; }
    = new List<IgnitionSnapshotEntity>();

    public ICollection<GatewaySnapshotEntity>
    GatewaySnapshots
    { get; set; }
    = new List<GatewaySnapshotEntity>();
}