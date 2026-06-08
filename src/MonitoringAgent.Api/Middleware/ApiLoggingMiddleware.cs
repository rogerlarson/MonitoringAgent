// ============================================================================
// Project: MonitoringAgent.Api
// File: ApiLoggingMiddleware.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Middleware that records API request activity and execution time.
//
//      Logs the HTTP method, request path, response status code, and total
//      request duration for every API request processed by the application.
// ============================================================================

using MonitoringAgent.Common.Interfaces;

namespace MonitoringAgent.Api.Middleware;

/// <summary>
/// Middleware that logs API request activity.
/// </summary>
public sealed class ApiLoggingMiddleware
{
    // =====================================================================
    // Dependencies
    // =====================================================================

    private readonly RequestDelegate _next;

    // =====================================================================
    // Constructor
    // =====================================================================

    /// <summary>
    /// Initializes a new instance of the middleware.
    /// </summary>
    /// <param name="next">
    /// Next middleware in the request pipeline.
    /// </param>
    public ApiLoggingMiddleware(
        RequestDelegate next)
    {
        _next =
            next;
    }

    // =====================================================================
    // Middleware Execution
    // =====================================================================

    /// <summary>
    /// Processes the HTTP request and records API activity.
    /// </summary>
    /// <param name="context">
    /// Current HTTP context.
    /// </param>
    /// <param name="logService">
    /// Logging service.
    /// </param>
    public async Task InvokeAsync(
        HttpContext context,
        ILogService logService)
    {
        var startUtc =
            DateTime.UtcNow;

        try
        {
            await _next(
                context);
        }
        finally
        {
            var elapsed =
                DateTime.UtcNow -
                startUtc;

            await logService.LogApi(
                $"{context.Request.Method} " +
                $"{context.Request.Path} " +
                $"{context.Response.StatusCode} " +
                $"{elapsed.TotalMilliseconds:F0}ms");
        }
    }
}