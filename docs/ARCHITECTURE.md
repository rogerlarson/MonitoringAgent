# Architecture Guide

## Overview

MonitoringAgent is a distributed monitoring platform designed to collect, store, evaluate, and visualize operational metrics from Windows servers and Ignition Gateway installations.

The platform consists of four primary applications:

- MonitoringAgent.Agent
- MonitoringAgent.Api
- MonitoringAgent.Engine
- MonitoringAgent.Web

and a shared SQL Server database.

---

# High-Level Architecture

```text
┌────────────────────────────┐
│     Monitored Server       │
│                            │
│  MonitoringAgent.Agent     │
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
│     MONITORING DB          │
└─────────────┬──────────────┘
              │
              │
     ┌────────┴────────┐
     │                 │
     ▼                 ▼
┌──────────────┐ ┌────────────────┐
│ Engine       │ │ Web Dashboard  │
│ (IIS)        │ │ (IIS)          │
└──────────────┘ └────────────────┘
```

---

# Component Responsibilities

## MonitoringAgent.Agent

Installed on monitored machines.

Responsibilities:

- Collect host metrics
- Collect disk metrics
- Collect network metrics
- Collect Ignition metrics
- Collect Gateway metrics
- Create HealthSnapshot payloads
- Publish snapshots to API

Runs as:

```text
Windows Service
```

Typical collection interval:

```text
60 seconds
```

---

## MonitoringAgent.Api

Central ingestion and query layer.

Responsibilities:

- Receive HealthSnapshot submissions
- Validate API keys
- Register new servers
- Store snapshots
- Expose REST endpoints
- Provide dashboard data
- Provide alert management APIs

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
- Scheduled jobs
- Health monitoring workers

Runs as:

```text
IIS Application
```

Engine status is recorded in:

```text
EngineServices
```

---

## MonitoringAgent.Web

React-based dashboard.

Responsibilities:

- Dashboard visualization
- Server management
- Alert management
- Historical analysis
- System administration

Runs as:

```text
IIS Static Site
```

---

# Snapshot Flow

The primary system workflow is metric collection.

```text
Agent
  |
  | Collect Metrics
  |
  v
HealthSnapshot
  |
  | POST /api/health
  |
  v
API
  |
  | Persist
  |
  v
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
Ignition
Gateway
Host Information
```

Creates:

```csharp
HealthSnapshot
```

Posts to:

```text
POST /api/health
```

API:

1. Validates request
2. Creates server if necessary
3. Updates LastSeenUtc
4. Stores HostSnapshot
5. Stores GatewaySnapshot
6. Stores IgnitionSnapshot

---

# Database Architecture

Core entities:

```text
ServerEntity
    |
    +-- HostSnapshotEntity
    |
    +-- ServerServiceEntity
            |
            +-- GatewaySnapshotEntity
            |
            +-- IgnitionSnapshotEntity
```

---

# Server Model

Represents a monitored machine.

```text
ServerEntity
```

Stores:

- Server Name
- Operating System
- Domain
- Agent Version
- Last Seen
- Status

One server can have:

```text
1..N Snapshots
1..N Services
1..N Alerts
```

---

# Service Model

Services are monitored components.

```text
ServiceEntity
```

Examples:

```text
Ignition
Gateway
```

A service may be attached to many servers.

Relationship:

```text
Service
   |
   +-- ServerService
   |
Server
```

---

# Host Metrics Storage

General operating system metrics are stored in:

```text
HostSnapshots
```

Contains:

- CPU %
- Memory %
- Available Memory
- Process Count
- Disk Usage
- Disk Latency
- Network Statistics
- System Uptime

---

# Ignition Metrics Storage

Ignition-specific data is stored in:

```text
IgnitionSnapshots
```

Contains:

- Service Running
- Process Running
- CPU %
- Memory
- Thread Count
- Handle Count
- Process ID
- Uptime
- Ignition Version
- Java Version

---

# Gateway Metrics Storage

Gateway availability metrics are stored in:

```text
GatewaySnapshots
```

Contains:

- Reachable
- Response Time
- HTTP Status Code

---

# Alert Architecture

## Alert Rules

Alert definitions.

Stored in:

```text
AlertRules
```

Example:

```text
CPU > 90%
Gateway Unreachable
Memory > 95%
```

Rules define:

- Metric
- Threshold
- Severity
- Sustain Time
- Repeat Time
- Auto Close Behavior

---

## Alert Events

Triggered alerts.

Stored in:

```text
AlertEvents
```

Lifecycle:

```text
Open
  |
  +--> Acknowledged
  |
  +--> Suppressed
  |
  +--> Closed
```

---

# Alert Evaluation Flow

```text
HostSnapshot
     |
     v
Alert Engine
     |
     v
Alert Rule Evaluation
     |
     +--> No Match
     |
     +--> Open Alert
                 |
                 +--> Notify
```

---

# Notification Flow

```text
Alert Opened
      |
      v
Email Service
      |
      v
SMTP Server
      |
      v
Recipients
```

Notification tracking:

```text
NotificationCount
LastNotificationUtc
```

---

# Engine Workers

The Engine hosts multiple worker services.

Worker status is tracked in:

```text
EngineServices
```

Each worker records:

- Status
- Run Count
- Error Count
- Last Success Time

---

# Dashboard Data Flow

```text
Browser
    |
    v
MonitoringAgent.Web
    |
    v
MonitoringAgent.Api
    |
    v
SQL Server
```

The dashboard never accesses SQL Server directly.

All requests pass through the API.

---

# Security Model

Current security mechanisms:

- HTTPS
- API Key validation
- IIS authentication controls
- SQL Server authentication

Agent requests include:

```text
X-API-Key
```

header when API key enforcement is enabled.

---

# Logging Architecture

Each application writes logs independently.

```text
Api/Logs
Engine/Logs
Agent/Logs
```

Log categories include:

- API Requests
- Engine Workers
- Alert Processing
- Snapshot Collection
- Email Delivery
- Errors

---

# Failure Handling

## Agent Offline

Detected using:

```text
LastSeenUtc
```

Server status becomes:

```text
Offline
```

---

## Gateway Failure

Detected when:

```text
GatewayReachable = false
```

Alert may be generated.

---

## Ignition Failure

Detected when:

```text
ProcessRunning = false
```

or

```text
ServiceRunning = false
```

Alert may be generated.

---

# Scalability Model

Current design supports:

```text
Many Agents
      |
      v
Single API
      |
      v
Single Database
```

Future scaling options:

```text
Multiple API Servers
Load Balancer
Separate Database Server
Dedicated Alert Engine Server
Agent Auto-Update Service
```

---

# Future Architecture Enhancements

Planned improvements:

- User authentication
- Role-based permissions
- Agent auto-updates
- High availability API
- Distributed Engine workers
- SMS notifications
- Teams notifications
- Slack notifications
- Reporting subsystem
- Scheduled exports
- Agent version management
- Self-monitoring health checks

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
Data      -> Entity Framework + Persistence
Common    -> Shared Models and Services
```
