using Microsoft.AspNetCore.Authentication.Negotiate;
using MonitoringAgent.Api.Data;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Services;
using MonitoringAgent.Api.Services.Interfaces;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Middleware;

// Get the Web Application builder object...
var builder = WebApplication.CreateBuilder(args);

// Add CORS...
builder.Services.AddCors(options =>
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

// Add Controllers...
builder.Services.AddControllers();

// Add Endpoints API Explorer...
builder.Services.AddEndpointsApiExplorer();

// Add Swagger Generator...
builder.Services.AddSwaggerGen();

// Add Logger Service...
builder.Services.AddSingleton<ILogService, LogService>();

// Add Logger Cleanup Service...
builder.Services.AddHostedService<LogCleanupService>();

// Add Alert Service...
builder.Services.AddScoped<AlertService>();

// Add Email Service...
builder.Services.AddScoped<IEmailService, EmailService>();

// Add Host Heartbeat Service...
builder.Services.AddHostedService<HostOfflineMonitorService>();

// Add Snapshot Retention/Cleanup Service...
builder.Services.AddHostedService<SnapshotCleanupService>();

// Configure Log Settings...
builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(
        "Logging"));

// Configure Email Settings...
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(
        "Email"));

// Configure Monitoring Settings...
var monitoringSettings =
    builder.Configuration
        .GetSection("Monitoring")
        .Get<MonitoringSettings>()
    ?? new MonitoringSettings();

builder.Services.AddSingleton(
    monitoringSettings);

// Configure Database Snapshot Retention Settings...
builder.Services.Configure<RetentionSettings>(
    builder.Configuration.GetSection(
        "Retention"));

// Add Monitoring Database Context Service...
builder.Services.AddDbContext<MonitoringDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "MonitoringDatabase")));

// Build Application...
var app = builder.Build();

// Log API Key authentication requirements initially... 
var logger =
    app.Services
        .GetRequiredService<
            ILogger<Program>>();

logger.LogInformation(
    "API key authentication {State}",
    monitoringSettings.RequireApiKey
        ? "ENABLED"
        : "DISABLED");

// Add the Swagger Page
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS Redirection...
app.UseHttpsRedirection();

// Use CORS...
app.UseCors("React");

// Add Middleware...
app.UseMiddleware<ApiLoggingMiddleware>();

// Map Controllers...
app.MapControllers();

// Run Application...
app.Run();
