namespace MonitoringAgent.Api.Data.Entities;

public sealed class ServerEntity
{
    public int ServerId { get; set; }

    public string ServerName
    { get; set; }
        = string.Empty;

    public DateTime? LastSeenUtc
    { get; set; }

    public string? Status
    { get; set; }

    public string? OperatingSystem
    { get; set; }

    public string? OperatingSystemVersion
    { get; set; }

    public int? ProcessorCount
    { get; set; }

    public long? TotalMemoryMb
    { get; set; }

    public string? AgentVersion
    { get; set; }

    public string? DomainName
    { get; set; }

    public DateTime CreatedDateUtc
    { get; set; }

    public ICollection<HostSnapshotEntity>HealthSnapshots
    { get; set; }
        = new List<HostSnapshotEntity>();

    public ICollection<ServerServiceEntity>ServerServices
    { get; set; }
        = new List<ServerServiceEntity>();
}