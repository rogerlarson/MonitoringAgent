# Database Guide

## Overview

The MonitoringAgent database stores monitoring data, service inventory, historical snapshots, alerting information, and engine execution state.

Database Engine:

```text
SQL Server
```

Persistence Layer:

```text
Entity Framework Core
```

All DateTime values are stored in UTC.

---

# Entity Relationship Diagram

```text
Server
 │
 ├── HostSnapshots
 │
 ├── AlertEvents
 │
 └── ServerServices
         │
         ├── GatewaySnapshots
         │
         └── IgnitionSnapshots

ServiceType
     │
     └── Service
             │
             └── ServerService

AlertRule
     │
     └── AlertEvent

EngineService
```

---

# Core Monitoring

## ServerEntity

Represents a monitored machine.

### Purpose

Stores inventory and current status information for monitored servers.

### Key Fields

```text
Id
Name
OperatingSystem
DomainName
AgentVersion
LastSeenUtc
Status
CreatedUtc
UpdatedUtc
```

### Relationships

```text
1 Server
    ├── Many HostSnapshots
    ├── Many AlertEvents
    └── Many ServerServices
```

---

## HostSnapshotEntity

Historical operating system metrics.

### Purpose

Stores collected host performance data from agents.

### Metrics

```text
CPU Usage
Memory Usage
Available Memory
Process Count
Disk Usage
Disk Queue Length
Disk Latency
Network Throughput
System Uptime
```

### Relationships

```text
Many HostSnapshots
        │
        └── 1 Server
```

### Retention

Controlled by:

```text
RetentionSettings
```

---

# Service Catalog

## ServiceTypeEntity

Defines available service categories.

### Examples

```text
Gateway
Ignition
```

### Purpose

Provides service classification.

---

## ServiceEntity

Represents a monitorable service.

### Examples

```text
Gateway
Ignition
```

### Relationships

```text
1 ServiceType
      │
      └── Many Services
```

---

## ServerServiceEntity

Associates services with servers.

### Purpose

Allows monitored services to be attached to servers.

### Relationships

```text
1 Server
1 Service
```

### Example

```text
Server: POLARIS
Service: Ignition
```

---

# Service Snapshots

## GatewaySnapshotEntity

Stores gateway availability history.

### Metrics

```text
Reachable
Response Time
HTTP Status Code
SnapshotUtc
```

### Relationships

```text
Many GatewaySnapshots
        │
        └── 1 ServerService
```

---

## IgnitionSnapshotEntity

Stores Ignition service performance history.

### Metrics

```text
Service Running
Process Running
CPU %
Memory Usage
Thread Count
Handle Count
Process Id
Uptime
Ignition Version
Java Version
```

### Relationships

```text
Many IgnitionSnapshots
        │
        └── 1 ServerService
```

---

# Alerting

## AlertRuleEntity

Defines monitoring thresholds.

### Examples

```text
CPU > 90%
Gateway Unreachable
Memory > 95%
```

### Configuration

```text
Metric
Threshold
Severity
SustainMinutes
RepeatMinutes
AutoClose
Enabled
```

### Relationships

```text
1 AlertRule
       │
       └── Many AlertEvents
```

---

## AlertEventEntity

Represents generated alerts.

### Lifecycle

```text
Open
Acknowledged
Suppressed
Closed
```

### Fields

```text
OpenedUtc
AcknowledgedUtc
SuppressedUtc
ClosedUtc
NotificationCount
LastNotificationUtc
```

### Relationships

```text
Many AlertEvents
        │
        ├── 1 Server
        └── 1 AlertRule
```

---

# Engine Monitoring

## EngineServiceEntity

Tracks worker execution health.

### Purpose

Provides visibility into background engine workers.

### Examples

```text
SnapshotAlertWorker
HostOfflineMonitorWorker
SnapshotCleanupWorker
LogCleanupWorker
```

### Fields

```text
Status
RunCount
ErrorCount
LastRunUtc
LastSuccessUtc
LastErrorUtc
LastErrorMessage
```

### Status Values

```text
Running
Stopped
Error
```

---

# Snapshot Lifecycle

```text
Agent
   │
   ▼
HealthSnapshot
   │
   ▼
API
   │
   ├── HostSnapshot
   ├── GatewaySnapshot
   └── IgnitionSnapshot
```

---

# Alert Lifecycle

```text
Snapshot
    │
    ▼
Alert Evaluation
    │
    ▼
Alert Rule
    │
    ├── No Match
    │
    └── Alert Event
             │
             ├── Acknowledge
             ├── Suppress
             └── Close
```

---

# Data Retention

Historical data retention is controlled through:

```text
RetentionSettings
```

Applies to:

```text
HostSnapshots
GatewaySnapshots
IgnitionSnapshots
```

Log retention is managed separately through:

```text
LogSettings.RetentionDays
```

---

# UTC Date Handling

All DateTime values are stored and retrieved as UTC.

Implemented through:

```text
UtcDateTimeConverter
NullableUtcDateTimeConverter
```

Applied automatically by:

```text
MonitoringDbContext
```

---

# Future Database Enhancements

## Planned

```text
Users
Roles
Permissions
AlertComments
AlertAssignments
```

## Future

```text
AuditLog
NotificationHistory
AgentInventory
AgentVersions
MaintenanceWindows
```
