// ============================================================================
// Project: MonitoringAgent.Api
// File: AlertsController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides alert management operations for active and historical alerts.
//
//      Supports alert retrieval, acknowledgement, suppression, closure,
//      alert statistics, and notification testing used by the monitoring
//      platform.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Requests;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Models;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides alert management operations.
/// </summary>
[ApiController]
[Route("api/alerts")]
public sealed class AlertsController
    : ControllerBase
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly MonitoringDbContext _db;
    private readonly IEmailService _emailService;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the controller.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    /// <param name="emailService">
    /// Email notification service.
    /// </param>
    public AlertsController(
        MonitoringDbContext db,
        IEmailService emailService)
    {
        _db =
            db;

        _emailService =
            emailService;
    }

    // =====================================================================
    // Alert Queries
    // =====================================================================

    /// <summary>
    /// Retrieves alert history.
    /// </summary>
    /// <param name="limit">
    /// Maximum number of alerts to return.
    /// </param>
    /// <returns>
    /// Alert collection.
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetAlerts(
        int limit = 500)
    {
        var alerts =
            await _db.AlertEvents
                .Include(x => x.Server)
                .Include(x => x.AlertRule)
                .OrderByDescending(
                    x => x.OpenedUtc)
                .Take(limit)
                .ToListAsync();

        return Ok(
            alerts.Select(
                MapAlert));
    }

    /// <summary>
    /// Retrieves currently active alerts.
    /// </summary>
    /// <param name="limit">
    /// Maximum number of alerts to return.
    /// </param>
    /// <returns>
    /// Active alert collection.
    /// </returns>
    [HttpGet("open")]
    public async Task<IActionResult> GetOpenAlerts(
        int limit = 100)
    {
        var alerts =
            await _db.AlertEvents
                .Include(x => x.Server)
                .Include(x => x.AlertRule)
                .Where(x =>
                    x.Status !=
                    AlertStatus.Closed)
                .OrderByDescending(
                    x => x.OpenedUtc)
                .Take(limit)
                .ToListAsync();

        return Ok(
            alerts.Select(
                MapAlert));
    }

    /// <summary>
    /// Retrieves a specific alert.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <returns>
    /// Alert details.
    /// </returns>
    [HttpGet("{alertId:long}")]
    public async Task<IActionResult> GetAlert(
        long alertId)
    {
        var alert =
            await _db.AlertEvents
                .Include(x => x.Server)
                .Include(x => x.AlertRule)
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

    // =====================================================================
    // Alert Acknowledgement
    // =====================================================================

    /// <summary>
    /// Acknowledges an alert.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <param name="request">
    /// Acknowledgement request.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
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

        // TODO:
        // Replace with authenticated user identity once authentication
        // is implemented.
        alert.AcknowledgedBy =
            string.IsNullOrWhiteSpace(
                Environment.UserName)
                    ? "System"
                    : Environment.UserName;

        await _db.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Removes acknowledgement from an alert.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPost("{alertId}/unacknowledge")]
    public async Task<IActionResult> Unacknowledge(
        int alertId)
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

        if (alert.Status == AlertStatus.Acknowledged)
        {
            // Reopen the alert.
            alert.Status =
                AlertStatus.Open;

            // Clear acknowledgement information.
            alert.AcknowledgedUtc =
                null;

            alert.AcknowledgedBy =
                null;

            await _db.SaveChangesAsync();
        }

        return Ok();
    }

    // =====================================================================
    // Alert Suppression
    // =====================================================================

    /// <summary>
    /// Suppresses an alert for a specified duration.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <param name="request">
    /// Suppression request.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
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

        alert.SuppressedBy =
            string.IsNullOrWhiteSpace(
                request.UserName)
                    ? Environment.UserName
                    : request.UserName;

        alert.SuppressedUntilUtc =
            DateTime.UtcNow.AddHours(
                request.Hours);

        alert.SuppressedUtc =
            DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Removes suppression from an alert.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPost("{alertId:long}/unsuppress")]
    public async Task<IActionResult> Unsuppress(
        int alertId)
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

        if (alert.Status == AlertStatus.Suppressed)
        {
            // Reopen the alert.
            alert.Status =
                AlertStatus.Open;

            // Clear suppression details.
            alert.SuppressedUntilUtc =
                null;

            alert.SuppressedBy =
                null;

            alert.SuppressedUtc =
                null;

            await _db.SaveChangesAsync();
        }

        return Ok();
    }

    // =====================================================================
    // Alert Closure
    // =====================================================================

    /// <summary>
    /// Closes an alert.
    /// </summary>
    /// <param name="alertId">
    /// Alert identifier.
    /// </param>
    /// <param name="request">
    /// Close request.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPost("{alertId:long}/close")]
    public async Task<IActionResult> Close(
        long alertId,
        [FromBody]
        CloseAlertRequest request)
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

        alert.ClosedBy =
            string.IsNullOrWhiteSpace(
                request.UserName)
                    ? Environment.UserName
                    : request.UserName;

        await _db.SaveChangesAsync();

        return Ok();
    }

    // =====================================================================
    // Alert Statistics
    // =====================================================================

    /// <summary>
    /// Retrieves alert notification statistics.
    /// </summary>
    /// <returns>
    /// Alert statistics.
    /// </returns>
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

    // =====================================================================
    // Email Testing
    // =====================================================================

    /// <summary>
    /// Sends a test email notification.
    /// </summary>
    /// <returns>
    /// Success response.
    /// </returns>
    [HttpPost("test-email")]
    public async Task<IActionResult> TestEmail()
    {
        var alert =
            new AlertNotification
            {
                ServerName =
                    "Test Server",

                RuleName =
                    "Test Alert Rule",

                Severity =
                    AlertSeverity.Information
                        .ToString(),

                Message =
                    "SMTP Test Alert",

                OpenedUtc =
                    DateTime.UtcNow,

                OccurrenceCount =
                    1,

                NotificationCount =
                    1,

                LastSeenUtc =
                    DateTime.UtcNow
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

    // =====================================================================
    // Mapping Helpers
    // =====================================================================

    /// <summary>
    /// Converts an alert entity into an API response model.
    /// </summary>
    /// <param name="alert">
    /// Alert entity.
    /// </param>
    /// <returns>
    /// Alert response model.
    /// </returns>
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

            ServerName =
                alert.Server?.ServerName
                ?? string.Empty,

            RuleName =
                alert.AlertRule?.RuleName
                ?? string.Empty,

            Severity =
                alert.AlertRule?.Severity
                    .ToString()
                ?? string.Empty,

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

            SuppressedBy =
                alert.SuppressedBy,

            SuppressedUntilUtc =
                alert.SuppressedUntilUtc,

            ClosedBy =
                alert.ClosedBy,

            NotificationCount =
                alert.NotificationCount
        };
    }
}