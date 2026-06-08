# Ignition Monitoring Platform

**Author:** Roger Larson
**Created:** 06/07/2026

---

# Overview

The Ignition Monitoring Platform is a centralized monitoring and alerting solution designed to provide operational visibility into Ignition gateways, Windows servers, background services, and supporting infrastructure.

The platform collects server health data, gateway statistics, Ignition process metrics, and alert information, presenting it through a web-based dashboard built with React and TypeScript.

---

# Primary Features

## Dashboard

Provides a high-level operational view of the monitored environment.

Features include:

* Fleet-wide CPU, Memory, and Disk utilization
* Server health summaries
* Alert counts
* Gateway response metrics
* Worker service status
* Top problem servers

---

## Server Details

Primary troubleshooting view for individual servers.

Features include:

* Host metrics
* Gateway metrics
* Ignition metrics
* Open alerts
* Alert history
* Event timeline
* Historical performance charts
* Service status monitoring

---

## Alert Management

Monitor and manage active alerts.

Features include:

* Open alert monitoring
* Alert acknowledgements
* Alert suppression
* Manual alert closure
* Historical alert tracking

---

## Alert Rules

Configure monitoring and alert generation behavior.

Features include:

* Create rules
* Edit rules
* Enable or disable rules
* Configure thresholds
* Configure notification settings
* Configure sustain and repeat intervals

Supported metrics include:

* Host snapshot age
* Gateway snapshot age
* Ignition snapshot age
* CPU utilization
* Memory utilization
* Disk utilization
* Gateway response time

---

## Engine Status

Monitors background processing services.

Features include:

* Service status monitoring
* Run counts
* Error counts
* Last successful execution timestamps

---

# Architecture

The platform is composed of three primary layers.

## Monitoring Agents

Monitoring agents execute on monitored servers and collect:

* Host performance metrics
* Operating system information
* Service health information
* Ignition process metrics

Collected data is transmitted to the monitoring API at scheduled intervals.

---

## Monitoring API

The ASP.NET Core API serves as the central processing layer.

Responsibilities:

* Snapshot ingestion
* Historical data storage
* Alert rule evaluation
* Alert lifecycle management
* Notification processing
* Dashboard data aggregation

---

## Web Application

The React frontend provides:

* Operational dashboards
* Alert management
* Historical performance visualization
* Configuration management
* Diagnostic visibility

---

# Technology Stack

## Frontend

* React
* TypeScript
* Vite
* React Router

## Backend

* ASP.NET Core
* REST API

## Database

* SQL Server

---

# Application Modules

## Dashboard Module

Provides a fleet-wide operational overview of monitored systems.

Primary functions:

* Resource utilization visibility
* Alert visibility
* Gateway health visibility
* Worker service visibility

---

## Server Monitoring Module

Responsible for displaying current and historical server performance data.

Monitored categories:

* CPU utilization
* Memory utilization
* Disk utilization
* Network activity
* System uptime

---

## Gateway Monitoring Module

Tracks Ignition gateway accessibility and responsiveness.

Monitored metrics:

* Reachability
* HTTP status codes
* Response times

---

## Ignition Monitoring Module

Tracks Ignition process health and resource consumption.

Monitored metrics:

* Process status
* Service status
* Version information
* Memory consumption
* Thread counts
* Handle counts

---

## Alerting Module

Responsible for alert generation, tracking, notification, and lifecycle management.

Features:

* Rule evaluation
* Alert creation
* Alert acknowledgement
* Alert suppression
* Alert closure
* Notification tracking

---

# API Endpoints

## Dashboard

| Endpoint              | Description               |
| --------------------- | ------------------------- |
| GET /dashboard/trends | Dashboard summary metrics |

---

## Servers

| Endpoint                           | Description                 |
| ---------------------------------- | --------------------------- |
| GET /servers                       | List all monitored servers  |
| GET /servers/{id}                  | Retrieve server details     |
| GET /servers/{id}/history          | Historical host metrics     |
| GET /servers/{id}/gateway-history  | Historical gateway metrics  |
| GET /servers/{id}/ignition-history | Historical Ignition metrics |

---

## Alerts

| Endpoint                        | Description            |
| ------------------------------- | ---------------------- |
| GET /alerts                     | List alerts            |
| GET /alerts/{id}                | Alert details          |
| POST /alerts/{id}/acknowledge   | Acknowledge alert      |
| POST /alerts/{id}/unacknowledge | Remove acknowledgement |
| POST /alerts/{id}/suppress      | Suppress alert         |
| POST /alerts/{id}/unsuppress    | Remove suppression     |
| POST /alerts/{id}/close         | Close alert            |

---

## Alert Rules

| Endpoint           | Description      |
| ------------------ | ---------------- |
| GET /rules         | List alert rules |
| POST /rules        | Create rule      |
| PUT /rules/{id}    | Update rule      |
| DELETE /rules/{id} | Delete rule      |

---

## Engine Status

| Endpoint           | Description              |
| ------------------ | ------------------------ |
| GET /engine-status | Background worker status |

---

# Alert Lifecycle

Alert states are intentionally distinct.

## Open

The monitored condition currently violates a configured alert rule.

Notifications may be sent while the alert remains open.

---

## Acknowledged

An operator has reviewed and accepted ownership of the alert.

Purpose:

* Indicates awareness
* Indicates ownership
* Prevents duplicate investigation effort

---

## Suppressed

Notifications for the alert have been intentionally disabled.

Purpose:

* Maintenance windows
* Known issues
* Planned outages

---

## Closed

The alert is no longer active.

Closure may occur:

* Automatically
* Manually by an operator

---

# Alert State Behavior

| State        | Visible | Notifications Sent | Rule Evaluated |
| ------------ | ------- | ------------------ | -------------- |
| Open         | Yes     | Yes                | Yes            |
| Acknowledged | Yes     | No                 | Yes            |
| Suppressed   | Yes     | No                 | Yes            |
| Closed       | No      | No                 | No             |

## Notes

Acknowledged alerts remain active but suppress additional notifications.

Suppressed alerts remain active but intentionally disable notification delivery.

Closed alerts are considered resolved and no longer participate in alert processing.

---

# Snapshot Collection

Default collection intervals:

| Category          | Interval   |
| ----------------- | ---------- |
| Host Metrics      | 60 Seconds |
| Gateway Metrics   | 60 Seconds |
| Ignition Metrics  | 60 Seconds |
| Alert Evaluation  | 60 Seconds |
| Engine Monitoring | 60 Seconds |

Historical data may be aggregated when displayed in charts to improve performance.

---

# Database Overview

Primary entities include:

## Servers

Stores monitored server information.

Examples:

* Server name
* Domain
* Operating system
* Hardware inventory

---

## Host Snapshots

Stores periodic performance snapshots.

Examples:

* CPU
* Memory
* Disk
* Network metrics

---

## Gateway Snapshots

Stores Ignition gateway availability data.

Examples:

* Response times
* HTTP status
* Reachability

---

## Ignition Snapshots

Stores Ignition process health information.

Examples:

* Memory consumption
* Thread counts
* Handle counts

---

## Alert Rules

Stores monitoring rules and thresholds.

---

## Alert Events

Stores generated alert records and lifecycle history.

---

# Deployment

## Frontend

Built using:

* React
* TypeScript
* Vite

Deployment targets:

* IIS
* Nginx
* Static Web Hosting

---

## Backend

Built using:

* ASP.NET Core

Deployment targets:

* Windows Server
* IIS

---

## Database

* SQL Server

---

# Development Setup

## Frontend

Install dependencies:

```bash
npm install
```

Run development server:

```bash
npm run dev
```

Build production assets:

```bash
npm run build
```

---

## Backend

Run API:

```bash
dotnet run
```

---

# Monitoring Philosophy

The platform prioritizes:

1. Operational visibility
2. Alert accuracy
3. Historical analysis
4. Minimal configuration overhead

Alert rules should favor actionable conditions over informational noise.

The objective is to reduce alert fatigue while maintaining awareness of infrastructure health.

---

# Known Limitations

Current limitations include:

* Single-user administrative environment
* No authentication
* No role-based permissions
* No maintenance window support
* No notification escalation policies
* No distributed collectors
* No mobile-specific UI optimizations

---

# Versioning

Version format:

```text
MAJOR.MINOR.PATCH
```

Example:

```text
0.1.0
```

Guidelines:

* MAJOR = Breaking architectural changes
* MINOR = New functionality
* PATCH = Bug fixes and improvements

---

# Roadmap

## Planned

* Email notification management
* Microsoft Teams notifications
* SMS notifications
* Alert escalation policies
* Alert correlation
* Reporting
* Export functionality
* User authentication
* Role-based access control

---

## Future Considerations

* Multi-site monitoring
* Agent auto-update
* Distributed collectors
* Mobile-friendly dashboards
* Server groups
* Server tags
* Availability tracking
* Scheduled reporting
* Audit logging

---

# Change Log

## 0.1.0 - 06/07/2026

### Added

* Dashboard
* Server Details
* Alerts
* Alert Details
* Alert Rules
* Engine Status
* About Page

### Monitoring

* Host metrics
* Gateway metrics
* Ignition metrics
* Historical charting

### Alerting

* Alert lifecycle tracking
* Alert acknowledgement
* Alert suppression
* Alert closure

### UI

* Dashboard cards
* Historical charts
* Event timeline
* Shared navigation
* Shared page headers
* Application footer
* Version information

### Refactoring

* Component decomposition
* CSS separation
* Shared layout components
* Route organization
* Documentation improvements
