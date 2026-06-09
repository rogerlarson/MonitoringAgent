# MonitoringAgent Deployment Checklist

## Release Information

Version:

```text
_________________________
```

Deployment Date:

```text
_________________________
```

Deployment Owner:

```text
_________________________
```

---

# Pre-Deployment Validation

## Source Control

- [ ] All code committed
- [ ] Changes pushed to Azure DevOps
- [ ] Release tag created
- [ ] Release notes completed
- [ ] Changelog updated

## Database

- [ ] Database backup completed
- [ ] Backup file verified
- [ ] Migration scripts reviewed
- [ ] Migrations tested in non-production environment

## Configuration

- [ ] API configuration verified
- [ ] Engine configuration verified
- [ ] Agent configuration verified
- [ ] SMTP settings verified
- [ ] API keys verified
- [ ] Connection strings verified

## Build Validation

- [ ] API published successfully
- [ ] Engine published successfully
- [ ] Agent published successfully
- [ ] Web dashboard built successfully
- [ ] No build warnings requiring review

---

# Database Deployment

## Backup

- [ ] MonitoringAgent database backed up

Backup Location:

```text
_________________________
```

## Migration

- [ ] Database migrations applied
- [ ] Migration completed successfully
- [ ] Tables verified
- [ ] No migration errors detected

---

# API Deployment

## Deployment

- [ ] Stop IIS site
- [ ] Deploy API files
- [ ] Verify configuration files
- [ ] Start IIS site

## Validation

- [ ] API starts successfully
- [ ] Swagger accessible
- [ ] Database connection successful
- [ ] Logs generated

Test Endpoint:

```text
/api/dashboard/summary
```

---

# Engine Deployment

## Deployment

- [ ] Stop MonitoringAgentEngine service
- [ ] Deploy Engine files
- [ ] Verify configuration files
- [ ] Start MonitoringAgentEngine service

## Validation

- [ ] Windows Service running
- [ ] Engine logs generated
- [ ] EngineLifecycleService executed
- [ ] EngineServices table updating
- [ ] Workers executing successfully

Workers:

- [ ] EngineLifecycleService
- [ ] LogCleanupWorker
- [ ] HostOfflineMonitorWorker
- [ ] SnapshotAlertWorker
- [ ] SnapshotCleanupWorker

---

# Dashboard Deployment

## Deployment

- [ ] Build React application
- [ ] Deploy build output
- [ ] Verify IIS configuration

## Validation

- [ ] Dashboard loads
- [ ] Charts render
- [ ] Alerts page loads
- [ ] Server list loads
- [ ] Historical data displays

---

# Agent Deployment

## Deployment

- [ ] Stop MonitoringAgentAgent service
- [ ] Deploy Agent files
- [ ] Verify configuration files
- [ ] Start MonitoringAgentAgent service

## Validation

- [ ] Windows Service running
- [ ] Agent logs generated
- [ ] Lifecycle events recorded
- [ ] First snapshot published
- [ ] AgentVersion reported
- [ ] LastSeenUtc updating

Verify Log Entries:

- [ ] Monitoring Agent starting
- [ ] Monitoring Agent started
- [ ] First snapshot published successfully

---

# Post Deployment Validation

## System Health

- [ ] API healthy
- [ ] Engine healthy
- [ ] Dashboard operational
- [ ] Agent reporting
- [ ] Snapshot ingestion working
- [ ] Alerts processing

## Database

- [ ] HostSnapshots created
- [ ] GatewaySnapshots created
- [ ] IgnitionSnapshots created
- [ ] AlertEvents processing correctly

## Notifications

- [ ] Email test successful
- [ ] Alert notification test successful

## Logging

- [ ] API logs verified
- [ ] Engine logs verified
- [ ] Agent logs verified
- [ ] Log cleanup functioning

---

# Rollback Readiness

## Backup Validation

- [ ] Previous binaries archived
- [ ] Database backup verified
- [ ] Rollback procedure documented

Backup Location:

```text
_________________________
```

---

# Deployment Sign-Off

Deployment Completed By:

```text
_________________________
```

Date:

```text
_________________________
```

Notes:

```text
________________________________________________

________________________________________________

________________________________________________
```
