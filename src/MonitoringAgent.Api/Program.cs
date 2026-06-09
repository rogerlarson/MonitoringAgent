// ============================================================================
// Project: MonitoringAgent.Api
// File: Program.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/09/2026
// Description:
//      Application startup configuration for the MonitoringAgent API.
//
//      Responsibilities:
//      - Configure dependency injection
//      - Configure application settings
//      - Configure SQL Server database access
//      - Configure repositories and services
//      - Configure middleware pipeline
//      - Configure controller routing
//      - Configure Swagger documentation
// ============================================================================

using Microsoft.AspNetCore.Authentication.Negotiate;
using MonitoringAgent.Data;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Common.Services;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Middleware;
using MonitoringAgent.Data.Repositories;
using MonitoringAgent.Common.Interfaces;

// ============================================================================
// Application Builder
// ============================================================================

var builder =
    WebApplication.CreateBuilder(
        args);

// ============================================================================
// Cross-Origin Resource Sharing (CORS)
// ============================================================================

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            "React",
            policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

// ============================================================================
// MVC Services
// ============================================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// ============================================================================
// Core Services
// ============================================================================

builder.Services.AddSingleton<
    ILogService,
    LogService>();

builder.Services.AddScoped<
    IEmailService,
    EmailService>();

// ============================================================================
// Repository Services
// ============================================================================

builder.Services.AddScoped<
    IMonitoringRepository,
    MonitoringRepository>();

// ============================================================================
// Application Configuration
// ============================================================================

builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(
        "Logging"));

var apiSettings =
    builder.Configuration
        .GetSection(
            "Api")
        .Get<ApiSettings>()
    ?? new ApiSettings();

builder.Services.AddSingleton(
    apiSettings);

// ============================================================================
// Database Configuration
// ============================================================================

builder.Services.AddDbContext<
    MonitoringDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "MonitoringDatabase")));

// ============================================================================
// Build Application
// ============================================================================

var app =
    builder.Build();

// ============================================================================
// Startup Diagnostics
// ============================================================================

var logger =
    app.Services
        .GetRequiredService<
            ILogger<Program>>();

logger.LogInformation(
    "API key authentication {State}",
    apiSettings.RequireApiKey
        ? "ENABLED"
        : "DISABLED");

// ============================================================================
// Development Tooling
// ============================================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

// ============================================================================
// HTTP Pipeline
// ============================================================================

app.UseHttpsRedirection();

app.UseCors(
    "React");

// ============================================================================
// Middleware
// ============================================================================

app.UseMiddleware<
    ApiLoggingMiddleware>();

// ============================================================================
// Endpoint Routing
// ============================================================================

app.MapControllers();

// ============================================================================
// Application Startup
// ============================================================================

app.Run();