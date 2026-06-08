/*
===============================================================================
Program
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Application entry point for the MonitoringAgent Simulator.

Responsibilities:
- Load application configuration
- Register dependency injection services
- Configure simulator settings
- Register HttpClient support
- Start the simulator worker service

Notes:
The simulator is intended to generate synthetic monitoring data
and submit snapshots to the MonitoringAgent API for development,
testing, and demonstration purposes.

===============================================================================
*/

using MonitoringAgent.Simulator.Configuration;
using MonitoringAgent.Simulator;

IHost host =
    Host.CreateDefaultBuilder(args)

        // ---------------------------------------------------------------------
        // Dependency Injection Registration
        // ---------------------------------------------------------------------
        .ConfigureServices(
            (context, services) =>
            {
                //
                // Bind simulator configuration from appsettings.json.
                //
                services.Configure<
                    SimulatorSettings>(
                    context.Configuration.GetSection(
                        "Simulator"));

                //
                // Shared HttpClient factory used for communication
                // with the MonitoringAgent API.
                //
                services.AddHttpClient();

                //
                // Main background worker responsible for generating
                // and submitting simulated monitoring data.
                //
                services.AddHostedService<Worker>();
            })

        .Build();

await host.RunAsync();