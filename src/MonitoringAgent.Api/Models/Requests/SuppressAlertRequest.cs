namespace MonitoringAgent.Api.Models.Requests;

public sealed class SuppressAlertRequest
{
    public string UserName
    { get; set; }
        = string.Empty;

    public int Hours
    { get; set; }
        = 24;
}