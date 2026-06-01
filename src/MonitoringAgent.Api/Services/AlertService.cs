using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data;
using MonitoringAgent.Api.Data.Entities;
using MonitoringAgent.Api.Data.Enums;
using MonitoringAgent.Api.Services.Interfaces;

namespace MonitoringAgent.Api.Services;

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

    public async Task EvaluateHostSnapshot(
        int serverId,
        HostSnapshotEntity snapshot)
    {
        // Add Rules to Engine
        
        // CPU Percent...
        await EvaluateMetric(
            serverId,
            "High CPU",
            snapshot.CpuPercent,
            $"CPU usage is {snapshot.CpuPercent:F2}%");

        // Memory Percent...
        await EvaluateMetric(
            serverId,
            "High Memory",
            snapshot.MemoryPercent,
            $"Memory usage is {snapshot.MemoryPercent:F2}%");

        // Disk Percent Used...
        await EvaluateMetric(
            serverId,
            "Disk Full",
            snapshot.DiskPercentUsed,
            $"Disk usage is {snapshot.DiskPercentUsed:F2}%");

        // High Disk Queue...
        await EvaluateMetric(
            serverId,
            "High Disk Queue",
            snapshot.DiskQueueLength,
            $"Disk queue is {snapshot.DiskQueueLength:F2}");
    }

    public async Task EvaluateGatewaySnapshot(
    int serverId,
    GatewaySnapshotEntity snapshot)
    {
        await EvaluateBooleanMetric(
            serverId,
            "Gateway Down",
            snapshot.Reachable,
            "Gateway is unreachable");
    }

    public async Task EvaluateIgnitionSnapshot(
    int serverId,
    IgnitionSnapshotEntity snapshot)
    {
        await EvaluateBooleanMetric(
            serverId,
            "Ignition Down",
            snapshot.ProcessRunning,
            "Ignition process is not running");
    }

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
                    
                    await _emailService
                        .SendAlertOpened(
                            openAlert);
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

                    await _emailService
                        .SendAlertReminder(
                            openAlert);
                }
            }

            openAlert.OccurrenceCount++;
        }
        else
        {
            if (openAlert != null)
            {
                if (openAlert.OpenedUtc == default)
                {
                    openAlert.OpenedUtc =
                        openAlert.FirstTriggeredUtc
                        ?? openAlert.LastSeenUtc
                        ?? DateTime.UtcNow;
                }

                openAlert.Status =
                    AlertStatus.Closed;

                openAlert.ClosedUtc =
                    DateTime.UtcNow;

                openAlert.NotificationCount++;

                openAlert.ClosedBy = "System";

                await _emailService
                    .SendAlertClosed(
                        openAlert);
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
}