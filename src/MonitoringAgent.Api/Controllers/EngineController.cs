// ============================================================================
// Project: MonitoringAgent.Api
// File: EngineController.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides monitoring engine status and worker health information.
//
//      Exposes runtime status details for background services managed by
//      the monitoring engine including execution counts, error counts,
//      and recent activity information.
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Models.Responses;
using MonitoringAgent.Data;

namespace MonitoringAgent.Api.Controllers;

/// <summary>
/// Provides monitoring engine status operations.
/// </summary>
[ApiController]
[Route("api/engine")]
public sealed class EngineController
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
    public EngineController(
        MonitoringDbContext db)
    {
        _db =
            db;
    }

    // =====================================================================
    // Engine Status
    // =====================================================================

    /// <summary>
    /// Retrieves status information for all registered engine services.
    /// </summary>
    /// <returns>
    /// Collection of engine service status records.
    /// </returns>
    [HttpGet("status")]
    public async Task<
        List<EngineServiceResponse>>
        GetStatus()
    {
        return await _db.EngineServices
            .OrderBy(
                x => x.ServiceName)
            .Select(x =>
                new EngineServiceResponse
                {
                    ServiceName =
                        x.ServiceName,

                    Status =
                        x.Status,

                    RunCount =
                        x.RunCount,

                    ErrorCount =
                        x.ErrorCount,

                    LastSuccessUtc =
                        x.LastSuccessUtc
                })
            .ToListAsync();
    }
}