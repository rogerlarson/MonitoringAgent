namespace MonitoringAgent.Api.Configuration;

public sealed class EmailSettings
{
    public string Host
    { get; set; }
        = string.Empty;

    public int Port
    { get; set; }

    public string UserName
    { get; set; }
        = string.Empty;

    public string Password
    { get; set; }
        = string.Empty;

    public string FromAddress
    { get; set; }
        = string.Empty;

    public string ToAddress
    { get; set; }
        = string.Empty;

    public bool EnableSsl
    { get; set; } = true;
}