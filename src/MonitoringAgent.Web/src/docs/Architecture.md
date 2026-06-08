# Architecture

## High Level

Browser
    ↓
React + TypeScript
    ↓
ASP.NET Core API
    ↓
SQL Server

## Monitoring Flow

Monitoring Agent
    ↓
Host Snapshot Collection
    ↓
Gateway Snapshot Collection
    ↓
Ignition Snapshot Collection
    ↓
Alert Evaluation Engine
    ↓
Notifications

## Frontend Modules

- Dashboard
- Server Details
- Alerts
- Alert Rules
- Engine Status

## Backend Modules

- Monitoring
- Alert Processing
- Notification Processing
- Historical Storage