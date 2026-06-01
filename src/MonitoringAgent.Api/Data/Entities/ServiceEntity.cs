using MonitoringAgent.Api.Data.Enums;

namespace MonitoringAgent.Api.Data.Entities;

public sealed class ServiceEntity
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; }
        = string.Empty;

    public int ServiceTypeId { get; set; }

    public string? CollectorName { get; set; }

    public ServiceRegistrationMode
        RegistrationMode
    {
        get; set;
    }

    public DateTime CreatedDateUtc { get; set; }

    public ServiceTypeEntity ServiceType
    { get; set; }
        = null!;

    public ICollection<ServerServiceEntity>
        ServerServices
    { get; set; }
        = new List<ServerServiceEntity>();
}