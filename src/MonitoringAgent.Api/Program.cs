using Microsoft.AspNetCore.Authentication.Negotiate;
using MonitoringAgent.Data;
using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Common.Services;
using MonitoringAgent.Common.Configuration;
using MonitoringAgent.Api.Configuration;
using MonitoringAgent.Api.Middleware;
using MonitoringAgent.Data.Repositories;
using MonitoringAgent.Common.Interfaces;

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

// Add Monitoring Repository
builder.Services.AddScoped<IMonitoringRepository, MonitoringRepository>();

// Configure Log Settings...
builder.Services.Configure<LogSettings>(
    builder.Configuration.GetSection(
        "Logging"));

// Add Email Service...
builder.Services.AddScoped<
    IEmailService,
    EmailService>();

// Configure API Settings...
var apiSettings =
    builder.Configuration
        .GetSection("Api")
        .Get<ApiSettings>()
    ?? new ApiSettings();

builder.Services.AddSingleton(
    apiSettings);

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
    apiSettings.RequireApiKey
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
