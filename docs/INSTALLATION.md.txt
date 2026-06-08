# Installation Guide

## Overview

This document describes how to install and configure the MonitoringAgent platform.

The platform consists of:

* MonitoringAgent.Agent
* MonitoringAgent.Api
* MonitoringAgent.Engine
* MonitoringAgent.Web
* SQL Server Database

---

# Production Architecture

Typical production deployment:

```text
Monitoring Server
│
├── IIS
│   ├── MonitoringAgent.Api
│   ├── MonitoringAgent.Engine
│   └── MonitoringAgent.Web
│
└── SQL Server
```

Monitored Machines:

```text
Server A
└── MonitoringAgent.Agent

Server B
└── MonitoringAgent.Agent

Server C
└── MonitoringAgent.Agent
```

The API, Engine, Web Dashboard, and SQL Server are typically installed on the same monitoring server.

Only MonitoringAgent.Agent is deployed to monitored machines and installed as a Windows Service.

---

# Prerequisites

## Monitoring Server

Recommended:

* Windows Server 2022
* IIS
* ASP.NET Core Hosting Bundle
* .NET 9 Runtime
* SQL Server 2022
* 4 CPU Cores
* 8 GB RAM

Minimum:

* Windows Server 2019
* IIS
* ASP.NET Core Hosting Bundle
* .NET 9 Runtime
* SQL Server Express
* 2 CPU Cores
* 4 GB RAM

---

## Agent Hosts

Recommended:

* Windows Server 2019+
* .NET 9 Runtime
* Network access to Monitoring API

---

# Database Installation

## Create Database

```sql
CREATE DATABASE MonitoringAgent;
GO
```

---

## Configure Connection String

Add connection string to appsettings.json:

```json
{
  "ConnectionStrings": {
    "MonitoringDatabase":
      "Server=SERVERNAME;Database=MonitoringAgent;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

## Apply Entity Framework Migrations

From the solution root:

```bash
dotnet ef database update ^
    --project MonitoringAgent.Data ^
    --startup-project MonitoringAgent.Api
```

Verify all tables were created successfully.

---

# API Installation

## Publish API

```bash
dotnet publish MonitoringAgent.Api ^
    -c Release ^
    -o Publish\Api
```

---

## Configure API

appsettings.json

```json
{
  "ApiSettings": {
    "RequireApiKey": true,
    "ApiKey": "ReplaceWithStrongKey"
  }
}
```

---

## Configure Logging

```json
{
  "LogSettings": {
    "LogDirectory": "Logs",
    "RetentionDays": 30
  }
}
```

---

## Configure Email

```json
{
  "EmailSettings": {
    "Host": "smtp.company.com",
    "Port": 587,
    "UserName": "monitoring",
    "Password": "password",
    "FromAddress": "monitoring@company.com",
    "ToAddress": "operations@company.com",
    "EnableSsl": true
  }
}
```

---

## Deploy API

Copy published files to:

```text
C:\MonitoringAgent\Api
```

Configure IIS site:

```text
Site Name:
MonitoringAgentApi

Physical Path:
C:\MonitoringAgent\Api
```

---

# Engine Installation

## Publish Engine

```bash
dotnet publish MonitoringAgent.Engine ^
    -c Release ^
    -o Publish\Engine
```

---

## Deploy Engine

Copy published files to:

```text
C:\MonitoringAgent\Engine
```

Create an IIS site or IIS application:

```text
Site Name:
MonitoringAgentEngine

Physical Path:
C:\MonitoringAgent\Engine
```

Recommended Application Pool:

```text
.NET CLR Version:
No Managed Code

Start Mode:
AlwaysRunning

Idle Timeout:
0

Load User Profile:
True
```

Verify:

* EngineServices table updates
* Workers execute successfully
* No startup errors appear in logs

---

# Web Dashboard Installation

## Install Dependencies

```bash
npm install
```

---

## Configure API Endpoint

Create .env:

```text
VITE_API_URL=https://api-server
```

---

## Build

```bash
npm run build
```

---

## Deploy

Copy:

```text
dist/*
```

to:

```text
C:\MonitoringAgent\Web
```

Create IIS site:

```text
Site Name:
MonitoringAgentWeb

Physical Path:
C:\MonitoringAgent\Web
```

---

# Agent Installation

## Publish Agent

```bash
dotnet publish MonitoringAgent.Agent ^
    -c Release ^
    -o Publish\Agent
```

---

## Configure Agent

appsettings.json

```json
{
  "AgentSettings": {
    "CollectorUrl":
      "https://monitoring-server/api/health",

    "ApiKey":
      "ReplaceWithStrongKey",

    "PollIntervalSeconds":
      60,

    "IgnitionServiceName":
      "Ignition",

    "GatewayUrl":
      "http://localhost:8088",

    "HttpTimeoutSeconds":
      10,

    "MonitoredDrive":
      "C:",

    "NetworkInterfaceName":
      "Ethernet"
  }
}
```

---

## Deploy Agent

Copy files to:

```text
C:\MonitoringAgent\Agent
```

---

## Install Windows Service

```powershell
sc create MonitoringAgentAgent ^
binPath= "C:\MonitoringAgent\Agent\MonitoringAgent.Agent.exe"
```

Start:

```powershell
sc start MonitoringAgentAgent
```

Verify:

```powershell
Get-Service MonitoringAgentAgent
```

---

## Verify Agent Reporting

Confirm:

* Server appears in dashboard
* HostSnapshots are created
* LastSeenUtc updates
* AgentVersion is reported

---

# Agent Upgrades

Because the Agent runs as a Windows Service, the executable cannot be replaced while running.

Stop service:

```powershell
Stop-Service MonitoringAgentAgent
```

Deploy new files.

Start service:

```powershell
Start-Service MonitoringAgentAgent
```

Or use:

```powershell
deployment\Upgrade-Agent.ps1
```

Verify:

* Service is running
* Server reports successfully
* New version appears in dashboard

---

# Verification Checklist

## Database

* [ ] Database created
* [ ] Migrations applied
* [ ] Connection string verified

## API

* [ ] IIS site created
* [ ] API starts successfully
* [ ] Database connection succeeds
* [ ] Logs are generated

## Engine

* [ ] IIS site created
* [ ] Engine starts successfully
* [ ] EngineServices table updates

## Dashboard

* [ ] Dashboard loads
* [ ] Server list displays
* [ ] Charts load
* [ ] Alerts display correctly

## Agent

* [ ] Service installed
* [ ] Service running
* [ ] Snapshots arrive at API
* [ ] Server appears in dashboard

---

# Troubleshooting

## Agent Not Reporting

Verify:

* CollectorUrl
* ApiKey
* Firewall rules
* HTTPS certificates

Check logs for:

```text
Failed to publish snapshot
```

---

## API Returns Unauthorized

Verify:

```text
AgentSettings.ApiKey
```

matches:

```text
ApiSettings.ApiKey
```

---

## Database Errors

Verify:

* SQL Server is running
* Connection string is correct
* EF migrations applied successfully

---

## Dashboard Cannot Connect

Verify:

* API URL is correct
* CORS configuration is correct
* IIS bindings are correct

---

# Upgrade Procedure

1. Backup database
2. Stop IIS API site
3. Stop IIS Engine site
4. Deploy updated binaries
5. Apply EF migrations
6. Start IIS API site
7. Start IIS Engine site
8. Upgrade agents if required
9. Verify dashboard functionality

For detailed deployment procedures, see:

```text
docs/DEPLOYMENT.md
```

---

# Support

For troubleshooting collect:

* API logs
* Engine logs
* Agent logs
* SQL Server logs

before investigating issues.
