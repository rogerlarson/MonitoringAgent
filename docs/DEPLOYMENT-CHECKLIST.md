# MonitoringAgent Deployment Checklist

## Before Deployment

- [ ] Backup database
- [ ] Verify migration scripts
- [ ] Verify publish completed successfully
- [ ] Verify appsettings.json changes
- [ ] Verify SMTP settings
- [ ] Verify API keys

---

## API Deployment

- [ ] Stop IIS site
- [ ] Copy API files
- [ ] Start IIS site
- [ ] Verify API responds

Test:

https://server/api/dashboard/summary

---

## Web Deployment

- [ ] Build React app
- [ ] Copy dist files
- [ ] Verify dashboard loads
- [ ] Verify charts load
- [ ] Verify alerts page loads

---

## Engine Deployment

- [ ] Stop Engine service
- [ ] Copy Engine files
- [ ] Start Engine service
- [ ] Verify EngineServices updated

---

## Agent Deployment

- [ ] Stop Agent service
- [ ] Copy Agent files
- [ ] Start Agent service
- [ ] Verify LastSeenUtc updates

---

## Post Deployment

- [ ] Dashboard operational
- [ ] API healthy
- [ ] Engine healthy
- [ ] Agent reporting
- [ ] Email test successful
- [ ] Alerts processing

---

## Rollback Prepared

- [ ] Previous binaries archived
- [ ] Database backup verified
- [ ] Rollback steps documented
