# Deployment Guide

## Overview

This document describes deployment procedures for the MonitoringAgent platform.

Production deployment consists of:

* MonitoringAgent.Api
* MonitoringAgent.Engine
* MonitoringAgent.Web
* MonitoringAgent.Agent
* SQL Server Database

Deployment targets:

| Component     | Hosting         |
| ------------- | --------------- |
| API           | IIS             |
| Engine        | IIS             |
| Web Dashboard | IIS             |
| Agent         | Windows Service |
| Database      | SQL Server      |

---

# Production Architecture

Monitoring Server

```text
C:\MonitoringAgent
│
├── Api
│   ├── MonitoringAgent.Api.exe
│   ├── appsettings.json
│   └── Logs
│
├── Engine
│   ├── MonitoringAgent.Engine.exe
│   ├── appsettings.json
│   └── Logs
│
└── Web
    ├── index.html
    ├── assets
    └── web.config
```

Monitored Servers

```text
C:\MonitoringAgent
│
└── Agent
    ├── MonitoringAgent.Agent.exe
    ├── appsettings.json
    └── Logs
```

---

# Database Deployment

## Backup Existing Database

Before deployment:

```sql
BACKUP DATABASE MonitoringAgent
TO DISK = 'D:\Backups\MonitoringAgent.bak'
```

---

## Apply Migrations

```bash
dotnet ef database update ^
    --project MonitoringAgent.Data ^
    --startup-project MonitoringAgent.Api
```

Verify migration success before deploying application binaries.

---

# API Deployment (IIS)

## Publish

```bash
dotnet publish MonitoringAgent.Api ^
    -c Release ^
    -o Publish\Api
```

---

## Install .NET Hosting Bundle

Install:

* ASP.NET Core Hosting Bundle

Required on every IIS server hosting the API.

---

## Deploy

Copy published files to:

```text
C:\MonitoringAgent\Api
```

---

## Create IIS Site

Example:

```text
Site Name:
MonitoringAgentApi

Physical Path:
C:\MonitoringAgent\Api

Port:
5001
```

---

## Application Pool

Recommended settings:

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

---

## HTTPS

Install certificate.

Bind:

```text
https://monitoring.company.com
```

---

## Verify

Browse to:

```text
https://monitoring.company.com/api/dashboard/summary
```

Verify JSON data is returned.

---

# Engine Deployment (IIS)

## Publish

```bash
dotnet publish MonitoringAgent.Engine ^
    -c Release ^
    -o Publish\Engine
```

---

## Deploy

Copy published files to:

```text
C:\MonitoringAgent\Engine
```

---

## Create IIS Site

Example:

```text
Site Name:
MonitoringAgentEngine

Physical Path:
C:\MonitoringAgent\Engine

Port:
5002
```

---

## Application Pool

Recommended settings:

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

---

## Verify

Confirm:

* EngineServices table updates
* Background workers execute successfully
* Logs are being generated
* No startup errors occur

---

# Dashboard Deployment (IIS)

## Build

```bash
npm install
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

---

## Create IIS Site

```text
Site Name:
MonitoringAgentWeb

Physical Path:
C:\MonitoringAgent\Web
```

---

## React SPA Rewrite

Create web.config:

```xml
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="React Routes">
          <match url=".*" />
          <conditions>
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```

---

## Verify

Navigate to:

```text
https://monitoring.company.com
```

Dashboard should load successfully.

Verify:

* Dashboard loads
* Charts render
* Alerts display
* Server list displays

---

# Agent Deployment

## Publish

```bash
dotnet publish MonitoringAgent.Agent ^
    -c Release ^
    -o Publish\Agent
```

---

## Deploy

Copy files to:

```text
C:\MonitoringAgent\Agent
```

on each monitored machine.

---

## Install Service

```powershell
sc create MonitoringAgentAgent ^
binPath= "C:\MonitoringAgent\Agent\MonitoringAgent.Agent.exe"
```

---

## Start Service

```powershell
sc start MonitoringAgentAgent
```

---

## Verify

Confirm:

* Service is running
* Server appears in dashboard
* LastSeenUtc updates
* HostSnapshots are created
* AgentVersion is reported

---

# Agent Upgrade Procedure

Because the Agent runs as a Windows Service, the executable cannot be replaced while the service is running.

Stop the service:

```powershell
Stop-Service MonitoringAgentAgent
```

Deploy updated files.

Start the service:

```powershell
Start-Service MonitoringAgentAgent
```

Or use:

```powershell
deployment\Upgrade-Agent.ps1
```

Verify:

* Agent service is running
* LastSeenUtc updates
* AgentVersion updated correctly

---

# Platform Upgrade Procedure

## Step 1

Backup:

* Database
* Configuration files
* Deployment packages

---

## Step 2

Stop IIS sites:

```powershell
Stop-Website MonitoringAgentApi
Stop-Website MonitoringAgentEngine
Stop-Website MonitoringAgentWeb
```

---

## Step 3

Deploy updated binaries:

```text
Api
Engine
Web
```

---

## Step 4

Apply database migrations:

```bash
dotnet ef database update ^
    --project MonitoringAgent.Data ^
    --startup-project MonitoringAgent.Api
```

---

## Step 5

Start IIS sites:

```powershell
Start-Website MonitoringAgentApi
Start-Website MonitoringAgentEngine
Start-Website MonitoringAgentWeb
```

---

## Step 6

Upgrade Agents if necessary.

---

## Step 7

Verify:

* API responds
* Engine workers running
* Dashboard loads
* Alerts processing
* Agents reporting

---

# Rollback Procedure

If deployment fails:

## Stop IIS Sites

```powershell
Stop-Website MonitoringAgentApi
Stop-Website MonitoringAgentEngine
Stop-Website MonitoringAgentWeb
```

---

## Restore Previous Build

Restore:

```text
Api
Engine
Web
```

from backup.

---

## Restore Database

```sql
RESTORE DATABASE MonitoringAgent
FROM DISK = 'D:\Backups\MonitoringAgent.bak'
```

---

## Start IIS Sites

```powershell
Start-Website MonitoringAgentApi
Start-Website MonitoringAgentEngine
Start-Website MonitoringAgentWeb
```

---

## Verify

Confirm:

* API accessible
* Dashboard accessible
* Engine workers healthy
* Alerts processing
* Agents reporting

---

# Production Checklist

Before release:

* [ ] Database backup completed
* [ ] Migrations tested
* [ ] API published
* [ ] Engine published
* [ ] Web dashboard built
* [ ] Agent published
* [ ] Configuration verified
* [ ] SMTP tested
* [ ] Alerts tested
* [ ] Dashboard verified
* [ ] Rollback plan prepared

---

# Recommended Future Improvements

Future deployments should eventually support:

* CI/CD pipelines
* GitHub Actions
* Azure DevOps
* Automated database migrations
* Blue/Green deployments
* Agent auto-upgrades
* Centralized agent upgrade management
* Agent version inventory dashboard
* Version compatibility checks
* Health-check based rollout validation
* Publish-All.ps1 deployment packaging
* Automated deployment verification

---

# Related Documentation

See also:

```text
README.md
docs/INSTALLATION.md
ROADMAP.md
CHANGELOG.md
deployment/DEPLOYMENT-CHECKLIST.md
```
