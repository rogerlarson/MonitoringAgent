# Deployment Guide

## Overview

This document describes deployment procedures for the MonitoringAgent platform.

Production deployment consists of:

- MonitoringAgent.Api
- MonitoringAgent.Engine
- MonitoringAgent.Web
- MonitoringAgent.Agent
- SQL Server Database

Deployment Targets:

| Component | Hosting |
|-----------|----------|
| API | IIS |
| Engine | Windows Service |
| Web Dashboard | IIS |
| Agent | Windows Service |
| Database | SQL Server |

---

# Production Architecture

## Monitoring Server

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

## Monitored Servers

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

## Backup Database

```sql
BACKUP DATABASE MonitoringAgent
TO DISK = 'D:\Backups\MonitoringAgent.bak'
```

## Apply Migrations

```bash
dotnet ef database update ^
    --project MonitoringAgent.Data ^
    --startup-project MonitoringAgent.Api
```

---

# API Deployment

## Publish

```bash
dotnet publish MonitoringAgent.Api ^
    -c Release ^
    -o Publish\Api
```

## Install Hosting Bundle

Install the ASP.NET Core Hosting Bundle on IIS servers.

## Deploy

Copy published files to:

```text
C:\MonitoringAgent\Api
```

## IIS Configuration

Application Pool:

```text
No Managed Code
AlwaysRunning
Idle Timeout = 0
Load User Profile = True
```

## Verify

```text
https://monitoring.company.com/api/dashboard/summary
```

---

# Engine Deployment

The Engine now runs as a Windows Service.

## Publish

```bash
dotnet publish MonitoringAgent.Engine ^
    -c Release ^
    -o Publish\Engine
```

## Deploy

Copy files to:

```text
C:\MonitoringAgent\Engine
```

## Install Service

```powershell
sc create MonitoringAgentEngine ^
binPath= "C:\MonitoringAgent\Engine\MonitoringAgent.Engine.exe"
```

## Start Service

```powershell
sc start MonitoringAgentEngine
```

## Verify

Confirm:

- Engine service running
- EngineServices table updating
- Worker activity recorded
- Logs generated

---

# Dashboard Deployment

## Build

```bash
npm install
npm run build
```

## Deploy

Copy build output to:

```text
C:\MonitoringAgent\Web
```

## Verify

Confirm:

- Dashboard loads
- Charts render
- Alerts display
- Server list loads

---

# Agent Deployment

## Publish

```bash
dotnet publish MonitoringAgent.Agent ^
    -c Release ^
    -o Publish\Agent
```

## Deploy

Copy files to:

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

Confirm:

- Service running
- Agent logs created
- Server appears in dashboard
- LastSeenUtc updates
- Snapshots created
- AgentVersion reported

---

# Agent Upgrade Procedure

```powershell
Stop-Service MonitoringAgentAgent
```

Deploy updated files.

```powershell
Start-Service MonitoringAgentAgent
```

Verify:

- Service running
- Version updated
- Reporting resumed

---

# Engine Upgrade Procedure

```powershell
Stop-Service MonitoringAgentEngine
```

Deploy updated files.

```powershell
Start-Service MonitoringAgentEngine
```

Verify:

- Service running
- Workers healthy
- Alerts processing

---

# Platform Upgrade Procedure

1. Backup database and configuration.
2. Stop Engine service.
3. Stop IIS websites.
4. Deploy API, Engine, and Web.
5. Apply migrations.
6. Start Engine service.
7. Start IIS websites.
8. Upgrade agents if required.
9. Verify system health.

---

# Logging Verification

Verify logs exist:

```text
Api\Logs
Engine\Logs
Agent\Logs
```

Expected daily format:

```text
log-YYYY-MM-DD.log
```

Verify startup entries:

```text
Application starting
Application started
```

Verify cleanup entries:

```text
Log cleanup complete
```

---

# Rollback Procedure

## Restore Applications

Restore previous deployment packages.

## Restore Database

```sql
RESTORE DATABASE MonitoringAgent
FROM DISK = 'D:\Backups\MonitoringAgent.bak'
```

## Restart Services

```powershell
Start-Service MonitoringAgentEngine
Start-Service MonitoringAgentAgent
```

Restart IIS websites.

---

# Production Checklist

- [ ] Database backup completed
- [ ] Migrations tested
- [ ] API published
- [ ] Engine published
- [ ] Web built
- [ ] Agent published
- [ ] Services installed
- [ ] Logging verified
- [ ] SMTP tested
- [ ] Alerts tested
- [ ] Dashboard verified
- [ ] Rollback plan prepared

---

# Azure DevOps Deployment

## Push Code

```bash
git add .
git commit -m "Release v0.8.0"
git push origin main
```

## Create Release Tag

```bash
git tag v0.8.0
git push origin v0.8.0
```

---

# Future Improvements

- CI/CD pipelines
- Azure DevOps release pipelines
- Automated migrations
- Blue/Green deployments
- Agent auto-upgrades
- Centralized agent management
- Version inventory dashboard
- Health-based rollout validation
- Publish-All.ps1 packaging

---

# Related Documentation

```text
README.md
ARCHITECTURE.md
CHANGELOG.md
ROADMAP.md
INSTALLATION.md
```
