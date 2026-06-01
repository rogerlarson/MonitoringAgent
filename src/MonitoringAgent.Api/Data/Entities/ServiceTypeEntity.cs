namespace MonitoringAgent.Api.Data.Entities;

public sealed class ServiceTypeEntity
{
    public int ServiceTypeId { get; set; }

    public string ServiceTypeName { get; set; }
        = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    public ICollection<ServiceEntity>
        Services
    { get; set; }
        = new List<ServiceEntity>();
}