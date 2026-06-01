using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Data.Entities;
using MonitoringAgent.Api.Data.Enums;
using MonitoringAgent.Api.Models.Requests;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Api.Services.Interfaces;

namespace MonitoringAgent.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public sealed class AlertsController
    : ControllerBase
{
    private readonly MonitoringDbContext _db;
    private readonly IEmailService _emailService;

    public AlertsController(
        MonitoringDbContext db,
        IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlerts()
    {
        var alerts =
            await _db.AlertEvents
                .OrderByDescending(
                    x => x.OpenedUtc)
                .ToListAsync();

        return Ok(
            alerts.Select(
                MapAlert));
    }

    [HttpGet("open")]
    public async Task<IActionResult> GetOpenAlerts()
    {
        var alerts =
            await _db.AlertEvents
                .Where(x =>
                    x.Status !=
                    AlertStatus.Closed)
                .OrderByDescending(
                    x => x.OpenedUtc)
                .ToListAsync();

        return Ok(
            alerts.Select(
                MapAlert));
    }

    [HttpGet("{alertId:long}")]
    public async Task<IActionResult> GetAlert(
        long alertId)
    {
        var alert =
            await _db.AlertEvents
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertEventId ==
                        alertId);

        if (alert == null)
        {
            return NotFound();
        }

        return Ok(
            MapAlert(
                alert));
    }

    [HttpPost("{alertId:long}/acknowledge")]
    public async Task<IActionResult> Acknowledge(
        long alertId,
        [FromBody]
        AcknowledgeAlertRequest request)
    {
        var alert =
            await _db.AlertEvents
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertEventId ==
                        alertId);

        if (alert == null)
        {
            return NotFound();
        }

        alert.Status =
            AlertStatus.Acknowledged;

        alert.AcknowledgedUtc =
            DateTime.UtcNow;

        alert.AcknowledgedBy =
            request.UserName;

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{alertId:long}/suppress")]
    public async Task<IActionResult> Suppress(
        long alertId,
        [FromBody]
        SuppressAlertRequest request)
    {
        var alert =
            await _db.AlertEvents
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertEventId ==
                        alertId);

        if (alert == null)
        {
            return NotFound();
        }

        alert.Status =
            AlertStatus.Suppressed;

        alert.SuppressedUntilUtc =
            DateTime.UtcNow.AddHours(
                request.Hours);

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("{alertId:long}/close")]
    public async Task<IActionResult> Close(
        long alertId)
    {
        var alert =
            await _db.AlertEvents
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertEventId ==
                        alertId);

        if (alert == null)
        {
            return NotFound();
        }

        alert.Status =
            AlertStatus.Closed;

        alert.ClosedUtc =
            DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok();
    }

    private static AlertResponse MapAlert(
        AlertEventEntity alert)
    {
        return new AlertResponse
        {
            AlertEventId =
                alert.AlertEventId,

            AlertRuleId =
                alert.AlertRuleId,

            ServerId =
                alert.ServerId,

            ServerServiceId =
                alert.ServerServiceId,

            Status =
                alert.Status.ToString(),

            Message =
                alert.Message,

            OpenedUtc =
                alert.OpenedUtc,

            ClosedUtc =
                alert.ClosedUtc,

            LastSeenUtc =
                alert.LastSeenUtc,

            FirstTriggeredUtc =
                alert.FirstTriggeredUtc,

            LastNotificationUtc =
                alert.LastNotificationUtc,

            OccurrenceCount =
                alert.OccurrenceCount,

            AcknowledgedUtc =
                alert.AcknowledgedUtc,

            AcknowledgedBy =
                alert.AcknowledgedBy,

            SuppressedUntilUtc =
                alert.SuppressedUntilUtc,

            NotificationCount =
                alert.NotificationCount
        };
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        return Ok(
            await _db.AlertEvents
                .Select(x => new
                {
                    x.AlertEventId,
                    x.NotificationCount,
                    x.LastNotificationUtc,
                    x.Status
                })
                .ToListAsync());
    }

    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var alert =
            new AlertEventEntity
            {
                AlertEventId =
                    999999,

                Message =
                    "SMTP Test Alert",

                OpenedUtc =
                    DateTime.UtcNow,

                OccurrenceCount =
                    1,

                NotificationCount =
                    1
            };

        await _emailService
            .SendAlertOpened(
                alert);

        return Ok(
            new
            {
                Message =
                    "Test email sent."
            });
    }
}