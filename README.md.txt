# MonitoringAgent

MonitoringAgent is a lightweight server and application monitoring platform built with .NET.

The platform consists of:

* Monitoring Agent (Windows Service)
* Monitoring API (ASP.NET Core)
* Monitoring Engine Workers
* Monitoring Database (SQL Server / EF Core)
* Monitoring Web Dashboard (React)

The system collects infrastructure metrics, application metrics, gateway availability, and service health information from monitored servers and stores historical snapshots for analysis and alerting.

---

## Features

### Host Monitoring

Collects:

* CPU utilization
* Memory utilization
* Available memory
* Process counts
* System uptime
* Context switches
* Page faults

### Disk Monitoring

Collects:

* Disk utilization
* Free disk space
* Read operations/sec
* Write operations/sec
* Read latency
* Write latency
* Disk queue length
* Average disk queue length

### Network Monitoring

Collects:

* Bytes received/sec
* Bytes sent/sec
* Receive errors
* Send errors
* Receive discards
* Send discards
* TCP retransmissions

### Ignition Monitoring

Collects:

* Ignition service state
* JVM process state
* CPU usage
* Memory usage
* Thread count
* Handle count
* Process uptime
* Ignition version
* Java version

### Gateway Monitoring

Collects:

* Reachability
* HTTP status code
* Response time

### Alerting

Supports:

* Warning alerts
* Critical alerts
* Sustained conditions
* Repeat notifications
* Alert acknowledgement
* Alert suppression
* Manual alert closure
* Email notifications

### Historical Metrics

Stores historical snapshots for:

* Hosts
* Ignition services
* Gateways

Enables:

* Trend analysis
* Dashboard charts
* Historical reporting

---

## Architecture

Agent

Server
→ Health Snapshot
→ API

API

Receives snapshots
→ Persists data
→ Evaluates alerts

Engine Workers

Background services responsible for:

* Alert processing
* Health calculations
* Cleanup tasks
* Maintenance operations

Database

Stores:

* Servers
* Services
* Snapshots
* Alerts
* Engine status

Web Dashboard

Displays:

* Server inventory
* Health status
* Active alerts
* Historical metrics
* Service monitoring

---

## Solution Structure

MonitoringAgent.Agent

Windows monitoring agent responsible for collecting and publishing metrics.

MonitoringAgent.Api

REST API used by agents, dashboard, and background services.

MonitoringAgent.Common

Shared models, interfaces, configuration objects, enums, and services.

MonitoringAgent.Data

Entity Framework Core persistence layer.

MonitoringAgent.Engine

Background processing and maintenance workers.

MonitoringAgent.Web

React dashboard application.

---

## Configuration

### Agent

AgentSettings

* CollectorUrl
* ApiKey
* PollIntervalSeconds
* IgnitionServiceName
* GatewayUrl
* HttpTimeoutSeconds
* MonitoredDrive
* NetworkInterfaceName
* IgnitionInstallPath

### API

ApiSettings

* RequireApiKey
* ApiKey

### Email

EmailSettings

* Host
* Port
* UserName
* Password
* FromAddress
* ToAddress
* EnableSsl

### Logging

LogSettings

* LogDirectory
* RetentionDays
* EnableApiLogging
* EnableHeartbeatLogging
* EnableAlertLogging
* EnableEmailLogging
* EnableMaintenanceLogging

---

## Security

Agent communication can be secured using API keys.

Agents include:

X-API-Key

with every snapshot request.

The API validates requests before accepting health data.

---

## Status

Current project status:

Active Development

Primary focus areas:

* Monitoring reliability
* Alert engine improvements
* Dashboard enhancements
* Service extensibility
