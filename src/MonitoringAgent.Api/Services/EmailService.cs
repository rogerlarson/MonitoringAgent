using Microsoft.Extensions.Options;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Data.Entities;
using MonitoringAgent.Api.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace MonitoringAgent.Api.Services;

public sealed class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogService _logService;

    public EmailService(
        IOptions<EmailSettings> options,
        ILogService logService)
    {
        _settings =
            options.Value;

        _logService =
            logService;
    }

    public async Task SendAlertOpened(
        AlertEventEntity alert)
    {
        await _logService.LogEmail(
            $"OPENED {alert.AlertEventId} {alert.Message}");

        var body =
            $@"Alert Opened

            Alert ID:
            {alert.AlertEventId}

            Message:
            {alert.Message}

            Opened:
            {alert.OpenedUtc:u}";

        await SendEmail(
            $"ALERT OPENED: {alert.Message}",
            body);
    }

    public async Task SendAlertReminder(
        AlertEventEntity alert)
    {
        await _logService.LogEmail(
            $"REMINDER {alert.AlertEventId} {alert.Message}");
        
        var body =
            $@"Alert Still Active

            Alert ID:
            {alert.AlertEventId}

            Message:
            {alert.Message}

            Occurrences:
            {alert.OccurrenceCount}

            Notifications:
            {alert.NotificationCount}";

        await SendEmail(
            $"ALERT REMINDER: {alert.Message}",
            body);
    }

    public async Task SendAlertClosed(
        AlertEventEntity alert)
    {
        await _logService.LogEmail(
            $"CLOSED {alert.AlertEventId} {alert.Message}");
        
        var body =
            $@"Alert Closed

            Alert ID:
            {alert.AlertEventId}

            Message:
            {alert.Message}

            Closed:
            {alert.ClosedUtc:u}";

        await SendEmail(
            $"ALERT CLOSED: {alert.Message}",
            body);
    }

    private async Task SendEmail(string subject, string body)
    {
        try
        {
            using var message =
                new MailMessage(
                    _settings.FromAddress,
                    _settings.ToAddress,
                    subject,
                    body);

            using var client =
                new SmtpClient(
                    _settings.Host,
                    _settings.Port);

            client.EnableSsl =
                _settings.EnableSsl;

            if (!string.IsNullOrWhiteSpace(
                    _settings.UserName))
            {
                client.Credentials =
                    new NetworkCredential(
                        _settings.UserName,
                        _settings.Password);
            }

            await _logService.LogEmail(
                $"Connecting to {_settings.Host}:{_settings.Port}");

            await client.SendMailAsync(
                message);

            await _logService.LogEmail(
                $"EMAIL SENT: {subject}");
        }
        catch (Exception ex)
        {
            await _logService.LogEmail(
                $"EMAIL FAILED: {ex}");
        }
    }
}