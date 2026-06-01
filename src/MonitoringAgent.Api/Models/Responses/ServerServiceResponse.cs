namespace MonitoringAgent.Api.Models.Responses;

public sealed class ServerServiceResponse
{
    public int ServerServiceId { get; set; }

    public int ServiceId { get; set; }

    public string ServiceName { get; set; }
        = string.Empty;

    public bool Enabled { get; set; }
}