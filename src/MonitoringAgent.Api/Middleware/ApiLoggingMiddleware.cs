using MonitoringAgent.Api.Services.Interfaces;

namespace MonitoringAgent.Api.Middleware;

public sealed class ApiLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ApiLoggingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

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