using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Services.Interfaces;

public interface IEmailService
{
    Task SendAlertOpened(
        AlertEventEntity alert);

    Task SendAlertReminder(
        AlertEventEntity alert);

    Task SendAlertClosed(
        AlertEventEntity alert);
}