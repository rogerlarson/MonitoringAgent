/*
===============================================================================
Worker
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Placeholder background worker for the MonitoringAgent Engine.

Responsibilities:
- Provide a hosted service entry point
- Maintain engine process lifetime
- Demonstrate worker execution flow

Current Status:
Template implementation.

Future engine responsibilities may include:

- Alert rule evaluation
- Alert lifecycle management
- Notification dispatch
- Auto-close processing
- Scheduled maintenance tasks
- Health monitoring

Notes:
This implementation currently performs no business logic and
simply emits periodic log messages.

===============================================================================
*/

namespace MonitoringAgent.Engine
{
    /// <summary>
    /// Background worker responsible for executing
    /// MonitoringAgent Engine tasks.
    ///
    /// Current implementation logs a heartbeat message
    /// once per second.
    /// </summary>
    public class Worker : BackgroundService
    {
        // ---------------------------------------------------------------------
        // Dependencies
        // ---------------------------------------------------------------------

        private readonly ILogger<Worker> _logger;

        public Worker(
            ILogger<Worker> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main worker execution loop.
        /// </summary>
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Worker running at: {time}",
                    DateTimeOffset.Now);

                await Task.Delay(
                    1000,
                    stoppingToken);
            }
        }
    }
}