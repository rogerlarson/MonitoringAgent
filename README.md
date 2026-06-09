# MonitoringAgent

A distributed monitoring platform for Windows infrastructure, Ignition Gateways, and industrial application environments.

MonitoringAgent provides agent-based metric collection, centralized data ingestion, historical storage, alerting, background processing, and a modern web dashboard for operational visibility.

---

# Platform Components

The platform consists of six primary projects:

| Project | Purpose |
|----------|----------|
| MonitoringAgent.Agent | Data collection agent installed on monitored servers |
| MonitoringAgent.Api | Central ingestion and query API |
| MonitoringAgent.Engine | Background processing and alert evaluation service |
| MonitoringAgent.Web | React dashboard application |
| MonitoringAgent.Data | Entity Framework Core persistence layer |
| MonitoringAgent.Common | Shared models, services, interfaces, and configuration |

---

# Current Release

```text
Version: 0.8.0
Status : Active Development
```

Current capabilities include:

- Host monitoring
- Disk monitoring
- Network monitoring
- Gateway monitoring
- Ignition monitoring
- Historical metrics
- Alert processing
- Email notifications
- Dashboard APIs
- Service monitoring
- Engine worker tracking
- Daily log management

---

# Core Features

## Host Monitoring

Collects:

- CPU utilization
- Memory utilization
- Available memory
- Process counts
- System uptime
- Context switches
- Page faults

## Disk Monitoring

Collects:

- Disk utilization
- Free disk space
- Read operations/sec
- Write operations/sec
- Read latency
- Write latency
- Disk queue length
- Average disk queue length

## Network Monitoring

Collects:

- Bytes received/sec
- Bytes sent/sec
- Receive errors
- Send errors
- Receive discards
- Send discards
- TCP retransmissions

## Ignition Monitoring

Collects:

- Service state
- Process state
- CPU utilization
- Memory utilization
- Thread count
- Handle count
- Process uptime
- Ignition version
- Java version

## Gateway Monitoring

Collects:

- Reachability
- HTTP status code
- Response time
- Availability status

---

# Alerting

MonitoringAgent supports:

- Warning alerts
- Critical alerts
- Sustained conditions
- Alert acknowledgements
- Alert suppression
- Alert closure
- Email notifications
- Repeat notifications

Alert lifecycle:

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

# Historical Data

Historical snapshots are stored for:

- Hosts
- Gateways
- Ignition Services

Used for:

- Trend analysis
- Dashboard charts
- Capacity planning
- Incident investigation
- Historical reporting

---

# Architecture

```text
Monitored Server
       |
       v
MonitoringAgent.Agent
       |
       | HTTPS
       v
MonitoringAgent.Api
       |
       v
SQL Server
       |
       +------------------+
       |                  |
       v                  v
MonitoringAgent.Engine    MonitoringAgent.Web
```

---

# Agent

Runs as:

```text
Windows Service
```

Responsibilities:

- Collect metrics
- Create HealthSnapshot payloads
- Publish snapshots
- Track agent version
- Record operational logs
- Manage local log retention

---

# API

Runs as:

```text
ASP.NET Core Application
```

Responsibilities:

- Receive snapshots
- Validate API keys
- Register servers
- Persist metrics
- Expose dashboard APIs
- Expose alert APIs

---

# Engine

Runs as:

```text
Windows Service
```

Responsibilities:

- Alert processing
- Host offline detection
- Snapshot cleanup
- Log cleanup
- Email notifications
- Scheduled maintenance

Current workers:

- EngineLifecycleService
- LogCleanupWorker
- HostOfflineMonitorWorker
- SnapshotAlertWorker
- SnapshotCleanupWorker

---

# Dashboard

Built with:

```text
React
```

Provides:

- Dashboard views
- Server inventory
- Active alerts
- Historical metrics
- Service monitoring
- Administrative functions

---

# Database

Primary entities:

```text
Servers
HostSnapshots
GatewaySnapshots
IgnitionSnapshots
Services
ServerServices
AlertRules
AlertEvents
EngineServices
```

---

# Logging

Each application maintains independent daily log files.

```text
Api/Logs
Engine/Logs
Agent/Logs
```

Format:

```text
log-YYYY-MM-DD.log
```

Categories include:

```text
API
ALERT
EMAIL
HEARTBEAT
ENGINE
AGENT
SYSTEM
```

Automatic log cleanup is supported through configurable retention settings.

---

# Security

Current security controls:

- HTTPS
- API Key Authentication
- SQL Server Authentication
- IIS Security Controls

Agent requests include:

```text
X-API-Key
```

when API key validation is enabled.

---

# Configuration

## AgentSettings

```text
CollectorUrl
ApiKey
PollIntervalSeconds
HttpTimeoutSeconds
GatewayUrl
IgnitionServiceName
IgnitionInstallPath
MonitoredDrive
NetworkInterfaceName
```

## ApiSettings

```text
RequireApiKey
ApiKey
```

## EmailSettings

```text
Host
Port
UserName
Password
FromAddress
ToAddress
EnableSsl
```

## LogSettings

```text
LogDirectory
RetentionDays
EnableApiLogging
EnableHeartbeatLogging
EnableAlertLogging
EnableEmailLogging
EnableMaintenanceLogging
```

---

# Documentation

Project documentation:

```text
README.md
ARCHITECTURE.md
INSTALLATION.md
DEPLOYMENT.md
DEPLOYMENT-CHECKLIST.md
CHANGELOG.md
```

---

# Roadmap

## Version 0.9.0

- Authentication
- User accounts
- Roles and permissions
- Alert comments
- Alert ownership

## Version 1.0.0

- Production release
- Documentation completion
- Dashboard polishing
- Installation package
- Agent auto-upgrade support

---

# Technology Stack

Backend:

```text
.NET 8
ASP.NET Core
Entity Framework Core
SQL Server
```

Frontend:

```text
React
Vite
```

Infrastructure:

```text
Windows Services
IIS
SMTP
```

---

# License

Internal project.
