namespace MonitoringAgent.Api.Models.Requests;

public sealed class AcknowledgeAlertRequest
{
    public string UserName
    { get; set; }
        = string.Empty;
}