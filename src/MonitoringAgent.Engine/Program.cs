/*
===============================================================================
Program
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Application entry point for the MonitoringAgent Engine.

Responsibilities:
- Configure dependency injection
- Configure database access
- Configure application settings
- Register hosted workers
- Initialize engine state
- Start background processing services

Hosted Workers:
- LogCleanupWorker
- HostOfflineMonitorWorker
- SnapshotAlertWorker
- SnapshotCleanupWorker

Services:
- AlertService
- EmailService
- EngineStatusService
- LogService

Database:
MonitoringDbContext

Execution Flow:

    Configuration
          ↓
    Dependency Injection
          ↓
    Database Initialization
          ↓
    Worker Registration
          ↓
    Engine Startup
          ↓
    Background Processing

Startup Behavior:
Any engine services marked as "Running" from a previous
execution are automatically marked as "Stopped" during
startup to ensure accurate service status reporting.

===============================================================================
*/

using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Common.Interfaces;
using MonitoringAgent.Common.Services;
using MonitoringAgent.Data;
using MonitoringAgent.Engine.Configuration;
using MonitoringAgent.Engine.Services;
using MonitoringAgent.Engine.Workers;

var builder =
    Host.CreateApplicationBuilder(
        args);

// -------------------------------------------------------------------------
// Core Services
// -------------------------------------------------------------------------

builder.Services.AddSingleton<
    ILogService,
    LogService>();

builder.Services.AddScoped<
    AlertService>();

builder.Services.AddScoped<
    IEmailService,
    EmailService>();

builder.Services.AddScoped<
    EngineStatusService>();

// -------------------------------------------------------------------------
// Background Workers
// -------------------------------------------------------------------------

builder.Services.AddHostedService<
    LogCleanupWorker>();

builder.Services.AddHostedService<
    HostOfflineMonitorWorker>();

builder.Services.AddHostedService<
    SnapshotAlertWorker>();

builder.Services.AddHostedService<
    SnapshotCleanupWorker>();

// -------------------------------------------------------------------------
// Configuration
// -------------------------------------------------------------------------

builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(
        "Logging"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(
        "Email"));

builder.Services.Configure<RetentionSettings>(
    builder.Configuration.GetSection(
        "Retention"));

var monitoringSettings =
    builder.Configuration
        .GetSection(
            "Monitoring")
        .Get<MonitoringSettings>()
    ?? new MonitoringSettings();

builder.Services.AddSingleton(
    monitoringSettings);

// -------------------------------------------------------------------------
// Database Configuration
// -------------------------------------------------------------------------

var connectionString =
    builder.Configuration.GetConnectionString(
        "MonitoringDatabase")
    ?? throw new InvalidOperationException(
        "Connection string 'MonitoringDatabase' not found.");

builder.Services.AddDbContext<
    MonitoringDbContext>(
    options =>
        options.UseSqlServer(
            connectionString));

var host =
    builder.Build();

// -------------------------------------------------------------------------
// Engine Startup Recovery
// -------------------------------------------------------------------------
//
// Any services that were previously marked as
// "Running" are transitioned to "Stopped" during
// startup. This prevents stale status records after
// crashes, reboots, or unexpected shutdowns.
//

using (var scope =
    host.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
            .GetRequiredService<
                MonitoringDbContext>();

    var runningServices =
        db.EngineServices
            .Where(x =>
                x.Status == "Running");

    foreach (var service in runningServices)
    {
        service.Status =
            "Stopped";
    }

    db.SaveChanges();
}

await host.RunAsync();