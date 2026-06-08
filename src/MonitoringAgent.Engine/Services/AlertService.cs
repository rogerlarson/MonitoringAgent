/*
===============================================================================
AlertService
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Central alert evaluation engine responsible for processing
monitoring data, managing alert lifecycle state transitions,
and dispatching notifications.

Responsibilities:
- Evaluate host metrics
- Evaluate gateway metrics
- Evaluate Ignition metrics
- Evaluate heartbeat conditions
- Evaluate snapshot age conditions
- Create alerts
- Open alerts
- Close alerts
- Suppress alerts
- Send notifications
- Send reminder notifications

Alert Lifecycle:

    Pending
        ↓
    Open
        ↓
    Acknowledged
        ↓
    Suppressed
        ↓
    Closed

Notification Lifecycle:

    Alert Opened
        ↓
    Reminder Notifications
        ↓
    Alert Closed

Notes:
This service represents the primary alerting engine
for the MonitoringAgent platform.

All alert creation and state transitions should
flow through this service.

===============================================================================
*/

// -------------------------------------------------------------------------
// Dependencies
// -------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Models;
using MonitoringAgent.Data;

namespace MonitoringAgent.Engine.Services;

public sealed class AlertService
{
    private readonly MonitoringDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ILogService _logService;

    public AlertService(
        MonitoringDbContext db,
        IEmailService emailService,
        ILogService logService)
    {
        _db = db;
        _emailService = emailService;
        _logService = logService;
    }

    // -------------------------------------------------------------------------
    // Host Snapshot Evaluation
    // -------
    public async Task EvaluateHostSnapshot(
    int serverId,
    HostSnapshotEntity snapshot)
    {
        await EvaluateMetric(
            serverId,
            "High CPU",
            snapshot.CpuPercent,
            $"CPU usage is {snapshot.CpuPercent:F2}%");

        await EvaluateMetric(
            serverId,
            "CPU Critical",
            snapshot.CpuPercent,
            $"CPU usage is {snapshot.CpuPercent:F2}%");

        await EvaluateMetric(
            serverId,
            "High Memory",
            snapshot.MemoryPercent,
            $"Memory usage is {snapshot.MemoryPercent:F2}%");

        await EvaluateMetric(
            serverId,
            "Memory Critical",
            snapshot.MemoryPercent,
            $"Memory usage is {snapshot.MemoryPercent:F2}%");

        await EvaluateMetric(
            serverId,
            "Disk Warning",
            snapshot.DiskPercentUsed,
            $"Disk usage is {snapshot.DiskPercentUsed:F2}%");

        await EvaluateMetric(
            serverId,
            "Disk Full",
            snapshot.DiskPercentUsed,
            $"Disk usage is {snapshot.DiskPercentUsed:F2}%");

        await EvaluateMetric(
            serverId,
            "High Disk Queue",
            snapshot.DiskQueueLength,
            $"Disk queue is {snapshot.DiskQueueLength:F2}");
    }

    // -------------------------------------------------------------------------
    // Gateway Snapshot Evaluation
    // -------------------------------------------------------------------------
    public async Task EvaluateGatewaySnapshot(
    int serverId,
    GatewaySnapshotEntity snapshot)
    {
        await EvaluateBooleanMetric(
            serverId,
            "Gateway Down",
            snapshot.Reachable,
            "Gateway is unreachable");

        await EvaluateMetric(
            serverId,
            "Gateway Slow",
            snapshot.ResponseMs,
            $"Gateway RTT is {snapshot.ResponseMs:F0} ms");
    }

    // -------------------------------------------------------------------------
    // Ignition Snapshot Evaluation
    // -------------------------------------------------------------------------
    public async Task EvaluateIgnitionSnapshot(
    int serverId,
    IgnitionSnapshotEntity snapshot)
    {
        bool ignitionHealthy =
            snapshot.ServiceRunning &&
            snapshot.ProcessRunning;

        await EvaluateBooleanMetric(
            serverId,
            "Ignition Down",
            ignitionHealthy,
            "Ignition service/process is not running");
    }

    // -------------------------------------------------------------------------
    // Boolean Threshold Evaluation
    // -------------------------------------------------------------------------
    private async Task EvaluateBooleanMetric(
    int serverId,
    string ruleName,
    bool value,
    string message)
    {
        var rule =
            await GetRule(
                ruleName);

        if (
            rule == null
            ||
            rule.ThresholdValue == null)
        {
            return;
        }

        bool threshold =
            rule.ThresholdValue.Value != 0;

        bool triggered =
            rule.Operator switch
            {
                AlertOperator.Equal =>
                    value == threshold,

                AlertOperator.NotEqual =>
                    value != threshold,

                _ => false
            };

        await ProcessRule(
            serverId,
            ruleName,
            triggered,
            message);
    }

    // -------------------------------------------------------------------------
    // Numeric Threshold Evaluation
    // -------------------------------------------------------------------------
    private async Task EvaluateMetric(
    int serverId,
    string ruleName,
    decimal value,
    string message)
    {
        var rule =
            await GetRule(
                ruleName);

        if (
            rule == null
            ||
            rule.ThresholdValue == null)
        {
            return;
        }

        bool triggered =
            EvaluateThreshold(
                value,
                rule.Operator,
                rule.ThresholdValue.Value);

        await ProcessRule(
            serverId,
            ruleName,
            triggered,
            $"{message} " +
            $"{rule.RuleName}: " +
            $"({value:F2} " +
            $"{FormatOperator(rule.Operator)} " +
            $"{rule.ThresholdValue:F2})");
    }

    // -------------------------------------------------------------------------
    // Alert Lifecycle Processing
    // -------------------------------------------------------------------------
    //
    // Handles:
    //
    // Pending → Open
    // Open → Reminder
    // Open → Closed
    // Pending → Closed
    //
    // Notification dispatch and lifecycle
    // transitions are performed here.
    //
    // This is the core alert engine.
    //
    private async Task ProcessRule(
       int serverId,
       string ruleName,
       bool triggered,
       string message)
    {
        var rule =
            await _db.AlertRules
                .FirstOrDefaultAsync(
                    x =>
                        x.RuleName ==
                        ruleName
                        &&
                        x.Enabled);

        if (rule == null)
        {
            return;
        }

        var openAlert =
            await _db.AlertEvents
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertRuleId ==
                        rule.AlertRuleId
                        &&
                        x.ServerId ==
                        serverId
                        &&
                        x.Status !=
                            AlertStatus.Closed);

        // Deal with unsuppressing after expiration of suppressed alert
        if (
            openAlert?.Status ==
                AlertStatus.Suppressed
            &&
            openAlert.SuppressedUntilUtc != null
            &&
            openAlert.SuppressedUntilUtc <=
                DateTime.UtcNow)
        {
            openAlert.Status =
                AlertStatus.Open;

            openAlert.SuppressedUntilUtc =
                null;

            openAlert.SuppressedBy =
                null;

            openAlert.SuppressedUtc =
                null;

            await _db.SaveChangesAsync();
        }

        if (triggered)
        {
            if (
                openAlert != null
                &&
                openAlert.Status ==
                    AlertStatus.Pending)
            {
                openAlert.LastSeenUtc =
                    DateTime.UtcNow;

                openAlert.Message = message;

                openAlert.OccurrenceCount++;

                var duration =
                    DateTime.UtcNow -
                    openAlert.FirstTriggeredUtc!.Value;

                if (
                    duration.TotalSeconds >=
                    rule.SustainSeconds)
                {
                    openAlert.Status =
                        AlertStatus.Open;

                    openAlert.OpenedUtc =
                        DateTime.UtcNow;

                    openAlert.LastNotificationUtc =
                        DateTime.UtcNow;

                    openAlert.NotificationCount++;
                    
                    await _logService.LogAlert(
                        $"OPENED AlertId={openAlert.AlertEventId} Rule={rule.RuleName} Message={openAlert.Message}");

                    await _db.SaveChangesAsync();

                    if (rule.EmailEnabled)
                    {
                        var notification =
                            await BuildNotification(
                                openAlert,
                                rule);

                        await _emailService
                            .SendAlertOpened(
                                notification);
                    }
                }

                return;
            }
            if (openAlert == null)
            {
                _db.AlertEvents.Add(
                    new AlertEventEntity
                    {
                        AlertRuleId =
                            rule.AlertRuleId,

                        ServerId =
                            serverId,

                        Status =
                            AlertStatus.Pending,

                        Message =
                            message,

                        FirstTriggeredUtc =
                            DateTime.UtcNow,

                        LastSeenUtc =
                            DateTime.UtcNow,

                        OccurrenceCount =
                            1,

                        NotificationCount =
                            0
                    });

                await _db.SaveChangesAsync();

                return;
            }

            openAlert.LastSeenUtc =
                DateTime.UtcNow;

            openAlert.Message = message;

            if (
                openAlert.Status ==
                    AlertStatus.Open)
            {
                if (
                    ShouldSendReminder(
                        openAlert,
                        rule))
                {
                    openAlert.LastNotificationUtc =
                        DateTime.UtcNow;

                    openAlert.NotificationCount++;

                    openAlert.SuppressedUntilUtc =
                        null;

                    openAlert.SuppressedBy =
                        null;

                    await _logService.LogAlert(
                        $"REMINDER AlertId={openAlert.AlertEventId} Rule={rule.RuleName} Message={openAlert.Message}");

                    await _db.SaveChangesAsync();

                    if (rule.EmailEnabled)
                    {
                        var notification =
                            await BuildNotification(
                                openAlert,
                                rule);

                        await _emailService
                            .SendAlertReminder(
                                notification);
                    }
                }
            }

            openAlert.OccurrenceCount++;
        }
        else
        {
            //
            // Pending alert cleared before sustain time.
            //
            if (
                openAlert != null &&
                openAlert.Status ==
                    AlertStatus.Pending)
            {
                openAlert.Status =
                    AlertStatus.Closed;

                openAlert.ClosedUtc =
                    DateTime.UtcNow;

                openAlert.ClosedBy =
                    "System";

                await _db.SaveChangesAsync();

                return;
            }

            //
            // Open alert resolved.
            //
            if (
                openAlert != null &&
                openAlert.Status ==
                    AlertStatus.Open)
            {
                openAlert.Status =
                    AlertStatus.Closed;

                openAlert.ClosedUtc =
                    DateTime.UtcNow;

                openAlert.NotificationCount++;

                openAlert.ClosedBy =
                    "System";

                await _logService.LogAlert(
                    $"CLOSED AlertId={openAlert.AlertEventId}");

                await _db.SaveChangesAsync();

                if (rule.EmailEnabled)
                {
                    var notification =
                        await BuildNotification(
                            openAlert,
                            rule);

                    await _emailService
                        .SendAlertClosed(
                            notification);
                }
            }
        }
    }

    private static bool EvaluateThreshold(
        decimal value,
        AlertOperator op,
        decimal threshold)
    {
        return op switch
        {
            AlertOperator.GreaterThan =>
                value > threshold,

            AlertOperator.GreaterThanOrEqual =>
                value >= threshold,

            AlertOperator.LessThan =>
                value < threshold,

            AlertOperator.LessThanOrEqual =>
                value <= threshold,

            AlertOperator.Equal =>
                value == threshold,

            AlertOperator.NotEqual =>
                value != threshold,

            _ => false
        };
    }

    public async Task EvaluateHeartbeat(
    int serverId,
    bool offline)
    {
        await ProcessRule(
            serverId,
            "Host Offline",
            offline,
            "Host has not reported recently");
    }

    public async Task EvaluateSnapshotAge(
        int serverId,
        string ruleName,
        DateTime snapshotUtc,
        string sourceName)
    {
        var ageSeconds =
            (decimal)(
                DateTime.UtcNow -
                snapshotUtc)
                .TotalSeconds;

        await EvaluateMetric(
            serverId,
            ruleName,
            ageSeconds,
            $"{sourceName} snapshot age is {ageSeconds:F0} seconds");
    }

    private static bool ShouldSendReminder(
    AlertEventEntity alert,
    AlertRuleEntity rule)
    {
        if (
            alert.LastNotificationUtc ==
            null)
        {
            return true;
        }

        return
            DateTime.UtcNow -
            alert.LastNotificationUtc.Value
            >=
            TimeSpan.FromSeconds(
                rule.RepeatSeconds);
    }

    private async Task<AlertRuleEntity?> GetRule(
    string ruleName)
    {
        return await _db.AlertRules
            .FirstOrDefaultAsync(
                x =>
                    x.RuleName ==
                    ruleName
                    &&
                    x.Enabled);
    }

    private static string FormatOperator(
    AlertOperator op)
    {
        return op switch
        {
            AlertOperator.GreaterThan => ">",
            AlertOperator.GreaterThanOrEqual => ">=",
            AlertOperator.LessThan => "<",
            AlertOperator.LessThanOrEqual => "<=",
            AlertOperator.Equal => "=",
            AlertOperator.NotEqual => "!=",
            _ => "?"
        };
    }

    private async Task<AlertNotification>BuildNotification(
        AlertEventEntity alert,
        AlertRuleEntity rule)
    {
        var server =
            await _db.Servers
                .AsNoTracking()
                .FirstAsync(
                    x =>
                        x.ServerId ==
                        alert.ServerId);

        return new AlertNotification
        {
            ServerName =
                server.ServerName,

            RuleName =
                rule.RuleName,

            Severity =
                rule.Severity.ToString(),

            Message =
                alert.Message,

            OpenedUtc =
                alert.OpenedUtc != default
                ? alert.OpenedUtc
                : alert.FirstTriggeredUtc ??
                    DateTime.UtcNow,

            OccurrenceCount =
                alert.OccurrenceCount,

            NotificationCount =
                alert.NotificationCount,

            LastSeenUtc =
                alert.LastSeenUtc
        };
    }
}