using Microsoft.Extensions.Options;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Models;
using System.Net;
using System.Net.Mail;

namespace MonitoringAgent.Common.Services;

/// <summary>
/// ============================================================================
/// Email Service
/// ============================================================================
///
/// Author: Roger Larson
/// Date: 06/07/2026
///
/// Sends email notifications for alert lifecycle events.
///
/// Supported Notifications:
/// - Alert Opened
/// - Alert Reminder
/// - Alert Closed
///
/// Responsibilities:
/// - Generate notification content
/// - Connect to SMTP server
/// - Send email messages
/// - Log email activity
///
/// Configuration:
/// - EmailSettings
///
/// Used By:
/// - AlertService
///
/// ============================================================================
/// </summary>
public sealed class EmailService
    : IEmailService
{
    // -------------------------------------------------------------------------
    // Dependencies
    // -------------------------------------------------------------------------

    private readonly EmailSettings _settings;
    private readonly ILogService _logService;

    // -------------------------------------------------------------------------
    // Constructor
    // -------------------------------------------------------------------------

    public EmailService(
        IOptions<EmailSettings> options,
        ILogService logService)
    {
        _settings =
            options.Value;

        _logService =
            logService;
    }

    // -------------------------------------------------------------------------
    // Alert Opened Notification
    // -------------------------------------------------------------------------

    public async Task SendAlertOpened(
        AlertNotification alert)
    {
        await _logService.LogEmail(
            $"OPENED {alert.ServerName} {alert.RuleName}");

        var body =
            $"""
            Alert Opened

            Server:
            {alert.ServerName}

            Rule:
            {alert.RuleName}

            Severity:
            {alert.Severity}

            Message:
            {alert.Message}

            Opened:
            {alert.OpenedUtc:u}

            Occurrences:
            {alert.OccurrenceCount}
            """;

        await SendEmail(
            $"[{alert.ServerName}] {alert.RuleName} ({alert.Severity})",
            body);
    }

    // -------------------------------------------------------------------------
    // Alert Reminder Notification
    // -------------------------------------------------------------------------

    public async Task SendAlertReminder(
        AlertNotification alert)
    {
        await _logService.LogEmail(
            $"REMINDER {alert.ServerName} {alert.RuleName}");

        var body =
            $"""
            Alert Reminder

            Server:
            {alert.ServerName}

            Rule:
            {alert.RuleName}

            Severity:
            {alert.Severity}

            Message:
            {alert.Message}

            Opened:
            {alert.OpenedUtc:u}

            Occurrences:
            {alert.OccurrenceCount}

            Notifications:
            {alert.NotificationCount}

            Last Seen:
            {alert.LastSeenUtc:u}
            """;

        await SendEmail(
            $"[{alert.ServerName}] {alert.RuleName} ({alert.Severity}) - REMINDER",
            body);
    }

    // -------------------------------------------------------------------------
    // Alert Closed Notification
    // -------------------------------------------------------------------------

    public async Task SendAlertClosed(
        AlertNotification alert)
    {
        await _logService.LogEmail(
            $"CLOSED {alert.ServerName} {alert.RuleName}");

        var body =
            $"""
            Alert Closed

            Server:
            {alert.ServerName}

            Rule:
            {alert.RuleName}

            Severity:
            {alert.Severity}

            Message:
            {alert.Message}

            Opened:
            {alert.OpenedUtc:u}

            Occurrences:
            {alert.OccurrenceCount}

            Notifications:
            {alert.NotificationCount}
            """;

        await SendEmail(
            $"[{alert.ServerName}] {alert.RuleName} RESOLVED",
            body);
    }

    // -------------------------------------------------------------------------
    // SMTP Email Delivery
    // -------------------------------------------------------------------------

    private async Task SendEmail(
        string subject,
        string body)
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

            //
            // Optional SMTP authentication.
            //
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