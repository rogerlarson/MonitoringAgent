namespace MonitoringAgent.Api.Services.Interfaces;

public interface ILogService
{
    Task LogAlert(
        string message);

    Task LogEmail(
        string message);

    Task LogHeartbeat(
        string message);

    Task LogApi(
        string message);

    Task LogMaintenance(
        string message);
}