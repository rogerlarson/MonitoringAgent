// ============================================================================
// Project: MonitoringAgent.Api
// File: AlertRulesController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides CRUD operations for alert rule management.
//
//      Alert rules define monitoring thresholds, severity levels,
//      notification behavior, and alert lifecycle settings used by the
//      monitoring engine during snapshot evaluation.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Requests;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Common.Entities;
using MonitoringAgent.Common.Enums;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides alert rule management operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class AlertRulesController
    : ControllerBase
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly MonitoringDbContext _db;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the controller.
    /// </summary>
    /// <param name="db">
    /// Database context.
    /// </param>
    public AlertRulesController(
        MonitoringDbContext db)
    {
        _db =
            db;
    }

    // =====================================================================
    // Query Operations
    // =====================================================================

    /// <summary>
    /// Retrieves all configured alert rules.
    /// </summary>
    /// <returns>
    /// List of alert rules.
    /// </returns>
    [HttpGet]
    public async Task<
        List<AlertRuleResponse>>
        GetRules()
    {
        return await _db.AlertRules
            .OrderBy(
                x => x.RuleName)
            .Select(x =>
                new AlertRuleResponse
                {
                    AlertRuleId =
                        x.AlertRuleId,

                    RuleName =
                        x.RuleName,

                    MetricName =
                        x.MetricName,

                    Operator =
                        x.Operator.ToString(),

                    Threshold =
                        x.ThresholdValue,

                    Severity =
                        x.Severity.ToString(),

                    SustainSeconds =
                        x.SustainSeconds,

                    RepeatSeconds =
                        x.RepeatSeconds,

                    AutoClose =
                        x.AutoClose,

                    Enabled =
                        x.Enabled,

                    EmailEnabled =
                        x.EmailEnabled
                })
            .ToListAsync();
    }

    // =====================================================================
    // Create Operations
    // =====================================================================

    /// <summary>
    /// Creates a new alert rule.
    /// </summary>
    /// <param name="request">
    /// Alert rule definition.
    /// </param>
    /// <returns>
    /// Success response.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        SaveAlertRuleRequest request)
    {
        var entity =
            new AlertRuleEntity
            {
                RuleName =
                    request.RuleName,

                MetricName =
                    request.MetricName,

                Operator =
                    Enum.Parse<AlertOperator>(
                        request.Operator),

                ThresholdValue =
                    request.Threshold,

                Severity =
                    Enum.Parse<AlertSeverity>(
                        request.Severity),

                SustainSeconds =
                    request.SustainSeconds,

                RepeatSeconds =
                    request.RepeatSeconds,

                AutoClose =
                    request.AutoClose,

                EmailEnabled =
                    request.EmailEnabled,

                Enabled =
                    request.Enabled
            };

        _db.AlertRules.Add(
            entity);

        await _db.SaveChangesAsync();

        return Ok();
    }

    // =====================================================================
    // Update Operations
    // =====================================================================

    /// <summary>
    /// Updates an existing alert rule.
    /// </summary>
    /// <param name="id">
    /// Alert rule identifier.
    /// </param>
    /// <param name="request">
    /// Updated alert rule definition.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        SaveAlertRuleRequest request)
    {
        var entity =
            await _db.AlertRules
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertRuleId == id);

        if (entity == null)
        {
            return NotFound();
        }

        entity.RuleName =
            request.RuleName;

        entity.MetricName =
            request.MetricName;

        entity.Operator =
            Enum.Parse<AlertOperator>(
                request.Operator);

        entity.ThresholdValue =
            request.Threshold;

        entity.Severity =
            Enum.Parse<AlertSeverity>(
                request.Severity);

        entity.SustainSeconds =
            request.SustainSeconds;

        entity.RepeatSeconds =
            request.RepeatSeconds;

        entity.AutoClose =
            request.AutoClose;

        entity.EmailEnabled =
            request.EmailEnabled;

        entity.Enabled =
            request.Enabled;

        await _db.SaveChangesAsync();

        return Ok();
    }

    // =====================================================================
    // Delete Operations
    // =====================================================================

    /// <summary>
    /// Deletes an alert rule.
    /// </summary>
    /// <param name="id">
    /// Alert rule identifier.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var entity =
            await _db.AlertRules
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertRuleId == id);

        if (entity == null)
        {
            return NotFound();
        }

        _db.AlertRules.Remove(
            entity);

        await _db.SaveChangesAsync();

        return Ok();
    }

    // =====================================================================
    // Enable / Disable Operations
    // =====================================================================

    /// <summary>
    /// Enables an alert rule.
    /// </summary>
    /// <param name="id">
    /// Alert rule identifier.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPost("{id}/enable")]
    public async Task<IActionResult> Enable(
        int id)
    {
        var entity =
            await _db.AlertRules
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertRuleId == id);

        if (entity == null)
        {
            return NotFound();
        }

        entity.Enabled = true;

        await _db.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Disables an alert rule.
    /// </summary>
    /// <param name="id">
    /// Alert rule identifier.
    /// </param>
    /// <returns>
    /// Success response or not found.
    /// </returns>
    [HttpPost("{id}/disable")]
    public async Task<IActionResult> Disable(
        int id)
    {
        var entity =
            await _db.AlertRules
                .FirstOrDefaultAsync(
                    x =>
                        x.AlertRuleId == id);

        if (entity == null)
        {
            return NotFound();
        }

        entity.Enabled = false;

        await _db.SaveChangesAsync();

        return Ok();
    }
}