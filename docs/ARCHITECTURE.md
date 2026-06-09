# Architecture Guide

## Overview

MonitoringAgent is a distributed monitoring platform designed to collect, store, evaluate, alert on, and visualize operational metrics from Windows servers and Ignition Gateway installations.

The platform consists of:

- MonitoringAgent.Agent
- MonitoringAgent.Api
- MonitoringAgent.Engine
- MonitoringAgent.Web
- Shared SQL Server Database

---

# High-Level Architecture

```text
┌────────────────────────────┐
│     Monitored Server       │
│                            │
│  MonitoringAgent.Agent     │
│   (Windows Service)        │
└─────────────┬──────────────┘
              │
              │ HTTPS
              ▼
┌────────────────────────────┐
│     MonitoringAgent.Api    │
│           (IIS)            │
└─────────────┬──────────────┘
              │
              ▼
┌────────────────────────────┐
│       SQL Server           │
│       Monitoring DB        │
└─────────────┬──────────────┘
              │
     ┌────────┴────────┐
     │                 │
     ▼                 ▼
┌──────────────┐ ┌────────────────┐
│ Engine       │ │ Web Dashboard  │
│ Windows Svc  │ │ React / IIS    │
└──────────────┘ └────────────────┘
```

---

# Component Responsibilities

## MonitoringAgent.Agent

Installed on monitored servers.

Responsibilities:

- Collect host metrics
- Collect disk metrics
- Collect network metrics
- Collect Ignition metrics
- Collect Gateway metrics
- Create HealthSnapshot payloads
- Publish snapshots to API
- Record lifecycle events
- Perform startup diagnostics
- Manage local log retention

Hosted Services:

- AgentLifecycleService
- Worker
- LogCleanupWorker

Runs as:

```text
Windows Service
```

---

## MonitoringAgent.Api

Central ingestion and query layer.

Responsibilities:

- Receive HealthSnapshot submissions
- Validate API keys
- Register new servers
- Store monitoring data
- Expose REST APIs
- Manage alerts
- Provide dashboard data

Runs as:

```text
IIS Application
```

---

## MonitoringAgent.Engine

Background processing engine.

Responsibilities:

- Alert evaluation
- Alert lifecycle management
- Notification processing
- Cleanup tasks
- Offline server detection
- Snapshot retention
- Worker health tracking

Hosted Workers:

- EngineLifecycleService
- LogCleanupWorker
- HostOfflineMonitorWorker
- SnapshotAlertWorker
- SnapshotCleanupWorker

Runs as:

```text
Windows Service
```

---

## MonitoringAgent.Web

React dashboard application.

Responsibilities:

- Dashboard visualization
- Server management
- Alert management
- Historical analysis
- Administrative functions

Runs as:

```text
IIS Hosted React Application
```

---

# Snapshot Flow

```text
Agent
  │
  ▼
HealthSnapshot
  │
  ▼
POST /api/health
  │
  ▼
API
  │
  ▼
Database
```

---

# Health Snapshot Processing

Agent collects:

```text
CPU
Memory
Disk
Network
Gateway
Ignition
Host Information
```

API processing:

1. Validate request
2. Validate API key
3. Create server if necessary
4. Update LastSeenUtc
5. Store HostSnapshot
6. Store GatewaySnapshot
7. Store IgnitionSnapshot

---

# Database Architecture

```text
ServerEntity
    │
    ├── HostSnapshotEntity
    │
    ├── AlertEventEntity
    │
    └── ServerServiceEntity
             │
             ├── GatewaySnapshotEntity
             │
             └── IgnitionSnapshotEntity
```

---

# Engine Service Monitoring

Engine worker health is stored in:

```text
EngineServices
```

Tracked values:

- Service Name
- Status
- Run Count
- Error Count
- Last Run UTC
- Last Success UTC
- Last Error UTC
- Last Error Message

---

# Server Status Model

Possible states:

```text
Healthy
Warning
Critical
Offline
```

Offline status is determined primarily from:

```text
LastSeenUtc
```

---

# Alert Architecture

## Alert Rules

Stored in:

```text
AlertRules
```

Rules define:

- Metric
- Threshold
- Severity
- Sustain Time
- Repeat Time
- Auto Close Behavior

## Alert Events

Stored in:

```text
AlertEvents
```

Lifecycle:

```text
Open
 ├─ Acknowledged
 ├─ Suppressed
 └─ Closed
```

---

# Notification Flow

```text
Alert Opened
      │
      ▼
Email Service
      │
      ▼
SMTP Server
      │
      ▼
Recipients
```

---

# Logging Architecture

Each application writes logs independently.

```text
Api/Logs
Engine/Logs
Agent/Logs
```

Daily files:

```text
log-YYYY-MM-DD.log
```

Categories:

```text
AGENT
API
ALERT
EMAIL
ENGINE
HEARTBEAT
SYSTEM
ERROR
```

Retention:

```text
LogSettings.RetentionDays
```

Cleanup is performed automatically by LogCleanupWorker.

---

# Security Model

Current security controls:

- HTTPS
- API Key Authentication
- IIS Authentication Controls
- SQL Server Authentication

Agent requests may include:

```text
X-API-Key
```

---

# Scalability Model

```text
Many Agents
      │
      ▼
Single API
      │
      ▼
Single Database
```

Future:

```text
Multiple API Servers
Load Balancer
Dedicated Database Server
Dedicated Engine Cluster
```

---

# Future Architecture Enhancements

## 0.9.0

- Authentication
- User Accounts
- Roles and Permissions
- Alert Comments
- Alert Ownership

## 1.0.0

- Production Release
- Installation Package
- Documentation Completion
- Dashboard Polish
- Agent Auto Upgrade Support

## Future

- Offline Snapshot Queue
- Agent Auto Registration
- Distributed Engine Execution
- SMS Notifications
- Teams Notifications
- Slack Notifications
- Reporting Subsystem
- Scheduled Exports

---

# Solution Structure

```text
MonitoringAgent.Agent
MonitoringAgent.Api
MonitoringAgent.Common
MonitoringAgent.Data
MonitoringAgent.Engine
MonitoringAgent.Web
```

Purpose:

```text
Agent     -> Data Collection
Api       -> Data Ingestion + Queries
Engine    -> Background Processing
Web       -> User Interface
Data      -> Persistence Layer
Common    -> Shared Models & Services
```
