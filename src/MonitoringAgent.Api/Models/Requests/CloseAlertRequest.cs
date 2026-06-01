namespace MonitoringAgent.Api.Models.Requests;

public sealed class CloseAlertRequest
{
    public string UserName
    { get; set; }
        = string.Empty;
}