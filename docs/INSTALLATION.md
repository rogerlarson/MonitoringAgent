# Installation Guide

## Overview

This document describes how to install and configure the MonitoringAgent platform.

The platform consists of:

- MonitoringAgent.Agent
- MonitoringAgent.Api
- MonitoringAgent.Engine
- MonitoringAgent.Web
- SQL Server Database

---

# Production Architecture

## Monitoring Server

```text
Monitoring Server
│
├── IIS
│   ├── MonitoringAgent.Api
│   └── MonitoringAgent.Web
│
├── MonitoringAgent.Engine
│   └── Windows Service
│
└── SQL Server
```

## Monitored Machines

```text
Server A
└── MonitoringAgent.Agent

Server B
└── MonitoringAgent.Agent

Server C
└── MonitoringAgent.Agent
```

The API and Web Dashboard are hosted in IIS.

The Engine and Agent run as Windows Services.

---

# Prerequisites

## Monitoring Server

Recommended:

- Windows Server 2022
- IIS
- ASP.NET Core Hosting Bundle
- .NET 8 Runtime
- SQL Server 2022
- 4 CPU Cores
- 8 GB RAM

Minimum:

- Windows Server 2019
- IIS
- ASP.NET Core Hosting Bundle
- .NET 8 Runtime
- SQL Server Express
- 2 CPU Cores
- 4 GB RAM

---

## Agent Hosts

Recommended:

- Windows Server 2019+
- .NET 8 Runtime
- Network access to Monitoring API

---

# Database Installation

## Create Database

```sql
CREATE DATABASE MonitoringAgent;
GO
```

## Configure Connection String

```json
{
  "ConnectionStrings": {
    "MonitoringDatabase":
      "Server=SERVERNAME;Database=MonitoringAgent;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Apply Migrations

```bash
dotnet ef database update ^
    --project MonitoringAgent.Data ^
    --startup-project MonitoringAgent.Api
```

---

# API Installation

## Publish

```bash
dotnet publish MonitoringAgent.Api ^
    -c Release ^
    -o Publish\Api
```

## Configure API

```json
{
  "Api": {
    "RequireApiKey": true,
    "ApiKey": "ReplaceWithStrongKey"
  }
}
```

## Configure Logging

```json
{
  "Logging": {
    "LogDirectory": "Logs",
    "RetentionDays": 30
  }
}
```

## Deploy

```text
C:\MonitoringAgent\Api
```

## Create IIS Site

```text
MonitoringAgentApi
```

Verify:

- API starts successfully
- Swagger loads
- Database connectivity works

---

# Engine Installation

## Publish

```bash
dotnet publish MonitoringAgent.Engine ^
    -c Release ^
    -o Publish\Engine
```

## Deploy

```text
C:\MonitoringAgent\Engine
```

## Install Windows Service

```powershell
sc create MonitoringAgentEngine ^
binPath= "C:\MonitoringAgent\Engine\MonitoringAgent.Engine.exe"
```

## Start Service

```powershell
sc start MonitoringAgentEngine
```

## Verify

Check:

- Engine logs created
- EngineServices table updating
- Workers executing

---

# Dashboard Installation

## Install Dependencies

```bash
npm install
```

## Configure Environment

```text
VITE_API_URL=https://monitoring.company.com
```

## Build

```bash
npm run build
```

## Deploy

```text
C:\MonitoringAgent\Web
```

## Configure IIS

Create IIS site:

```text
MonitoringAgentWeb
```

Verify:

- Dashboard loads
- API communication works
- Charts render

---

# Agent Installation

## Publish

```bash
dotnet publish MonitoringAgent.Agent ^
    -c Release ^
    -o Publish\Agent
```

## Configure Agent

```json
{
  "AgentSettings": {
    "CollectorUrl": "https://monitoring.company.com/api/health",
    "ApiKey": "ReplaceWithStrongKey",
    "PollIntervalSeconds": 60,
    "HttpTimeoutSeconds": 10,
    "GatewayUrl": "http://localhost:8088",
    "IgnitionServiceName": "Ignition",
    "MonitoredDrive": "C:"
  }
}
```

## Configure Logging

```json
{
  "Logging": {
    "LogDirectory": "Logs",
    "RetentionDays": 30
  }
}
```

## Deploy

```text
C:\MonitoringAgent\Agent
```

## Install Service

```powershell
sc create MonitoringAgentAgent ^
binPath= "C:\MonitoringAgent\Agent\MonitoringAgent.Agent.exe"
```

## Start Service

```powershell
sc start MonitoringAgentAgent
```

## Verify

```powershell
Get-Service MonitoringAgentAgent
```

Verify:

- Agent service running
- Agent logs generated
- Server appears in dashboard
- LastSeenUtc updates
- AgentVersion reported

---

# First-Time Validation

## Agent Logs

Verify:

```text
Monitoring Agent starting
Monitoring Agent started
First snapshot published successfully
```

## Engine Logs

Verify:

```text
Monitoring Agent Engine starting
Monitoring Agent Engine started
```

## Database

Verify:

- Servers table populated
- HostSnapshots created
- GatewaySnapshots created
- IgnitionSnapshots created

---

# Verification Checklist

## Database

- [ ] Database created
- [ ] Migrations applied
- [ ] Connection string verified

## API

- [ ] IIS site created
- [ ] API starts successfully
- [ ] Swagger accessible
- [ ] Logs generated

## Engine

- [ ] Windows Service installed
- [ ] Service running
- [ ] Engine workers active
- [ ] Logs generated

## Dashboard

- [ ] Dashboard loads
- [ ] Charts render
- [ ] Alerts display

## Agent

- [ ] Service installed
- [ ] Service running
- [ ] Logs generated
- [ ] Snapshots received

---

# Troubleshooting

## Agent Not Reporting

Verify:

- CollectorUrl
- ApiKey
- Firewall Rules
- HTTPS Certificates

Check Agent logs for:

```text
Failed to publish snapshot
Unable to communicate with monitoring API
```

---

## Engine Not Running

Verify:

```powershell
Get-Service MonitoringAgentEngine
```

Check:

```text
Engine Logs
Windows Event Viewer
```

---

## API Authentication Failures

Verify Agent API key matches API configuration.

---

## Dashboard Issues

Verify:

- API URL configuration
- IIS bindings
- CORS configuration

---

# Upgrade Procedure

1. Backup database.
2. Stop Engine service.
3. Stop API IIS site.
4. Deploy updated binaries.
5. Apply migrations.
6. Start Engine service.
7. Start API site.
8. Upgrade agents if required.
9. Validate dashboard.

---

# Support Information

Collect before troubleshooting:

- API logs
- Engine logs
- Agent logs
- SQL Server logs
- Application configuration

---

# Related Documentation

```text
ARCHITECTURE.md
DEPLOYMENT.md
CHANGELOG.md
ROADMAP.md
README.md
```
