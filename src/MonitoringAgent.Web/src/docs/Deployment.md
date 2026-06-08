# Deployment Guide

## Requirements

### Frontend

- Node.js 22+
- npm

### Backend

- .NET 9
- IIS Hosting Bundle

### Database

- SQL Server 2022

---

## Frontend Build

npm install

npm run build

Artifacts generated in:

dist/

---

## Backend Publish

dotnet publish -c Release

---

## IIS Configuration

### Frontend Site

Point IIS site root to:

dist/

### Backend Site

Point IIS application to:

publish/

---

## Environment Variables

Frontend:

VITE_API_URL

Backend:

ConnectionStrings__DefaultConnection

---

## Services

Required:

- Monitoring API
- Snapshot Collection Service
- Alert Evaluation Service

---

## Post Deployment Verification

Verify:

- Dashboard loads
- Server details load
- Alerts load
- Historical charts load
- Alert actions function
- Engine status loads