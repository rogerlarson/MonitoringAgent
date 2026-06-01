namespace MonitoringAgent.Api.Models.Responses;

public sealed class ServiceResponse
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; }
        = string.Empty;

    public string ServiceTypeName { get; set; }
        = string.Empty;
}