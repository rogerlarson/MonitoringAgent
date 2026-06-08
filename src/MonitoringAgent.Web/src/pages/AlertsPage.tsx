/**
 * ============================================================================
 * Alerts Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Centralized alert management interface.
 *
 * Features:
 * - View active alerts
 * - View historical alerts
 * - Filter and search alerts
 * - Navigate to alert details
 *
 * Notes:
 * Open alerts represent currently active conditions.
 * Historical alerts provide operational context and auditing.
 * ============================================================================
 */

// Styles
import "./AlertsPage.css";

// React
import {
    useEffect,
    useState
} from "react";

import {
    Link
} from "react-router-dom";

// APIs
import {
    getAlerts
} from "../api/alertsApi";

// Models
import type {
    AlertSummaryResponse
} from "../models/AlertSummaryResponse";

// Components
import DashboardCard from "../components/DashboardCard";
import LastUpdated from "../components/LastUpdated";

// Navigation
import AppNav 
    from "../nav/AppNav";
import PageHeader
    from "../components/PageHeader";
import Footer
    from "../nav/Footer";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

/**
 * Alert filter options.
 */
type AlertFilter =
    | "Open"
    | "Pending"
    | "Closed"
    | "Critical"
    | "Warning"
    | "All";

/**
 * Alerts Page
 *
 * Displays:
 * - Alert summary metrics
 * - Alert filtering controls
 * - Current and historical alert events
 *
 * Automatically refreshes every 30 seconds.
 */
export default function AlertsPage() {

    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("Alerts");

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [alerts, setAlerts] =
        useState<AlertSummaryResponse[]>([]);

    const [lastUpdated, setLastUpdated] =
        useState(new Date());

    const [filter, setFilter] =
        useState<AlertFilter>("Open");

    // -------------------------------------------------------------------------
    // Data Loading
    // -------------------------------------------------------------------------

    const loadData = () => {

        getAlerts()
            .then(alerts => {

                setAlerts(alerts);

                setLastUpdated(
                    new Date()
                );
            });
    };

    // -------------------------------------------------------------------------
    // Auto Refresh
    // -------------------------------------------------------------------------

    useEffect(() => {

        loadData();

        const timer =
            setInterval(
                loadData,
                30000
            );

        return () =>
            clearInterval(
                timer
            );

    }, []);

    // -------------------------------------------------------------------------
    // Alert Metrics
    // -------------------------------------------------------------------------

    const openAlerts =
        alerts.filter(
            alert =>
                alert.status === "Open"
        );

    const criticalAlerts =
        alerts.filter(
            alert =>
                alert.status !== "Closed" &&
                alert.severity === "Critical"
        );

    const pendingAlerts =
        alerts.filter(
            alert =>
                alert.status === "Pending"
        );

    const warningAlerts =
        alerts.filter(
            alert =>
                alert.status !== "Closed" &&
                alert.severity === "Warning"
        );

    const acknowledgedAlerts =
        alerts.filter(
            alert =>
                alert.status === "Acknowledged"
        );

    const suppressedAlerts =
        alerts.filter(
            alert =>
                alert.status === "Suppressed"
        );

    const closedToday =
        alerts.filter(alert => {

            if (!alert.closedUtc) {
                return false;
            }

            const closed =
                new Date(
                    alert.closedUtc
                );

            const today =
                new Date();

            return (
                closed.getFullYear() ===
                    today.getFullYear() &&
                closed.getMonth() ===
                    today.getMonth() &&
                closed.getDate() ===
                    today.getDate()
            );
        });

    // -------------------------------------------------------------------------
    // Filtered Alerts
    // -------------------------------------------------------------------------

    const filteredAlerts =
        alerts.filter(alert => {

            switch (filter) {

                case "Open":
                    return alert.status === "Open";

                case "Pending":
                    return alert.status === "Pending";

                case "Closed":
                    return alert.status === "Closed";

                case "Critical":
                    return (
                        alert.status !== "Closed" &&
                        alert.severity === "Critical"
                    );

                case "Warning":
                    return (
                        alert.status !== "Closed" &&
                        alert.severity === "Warning"
                    );

                default:
                    return true;
            }
        });

    // -------------------------------------------------------------------------
    // Filter Buttons
    // -------------------------------------------------------------------------

    const filterButtons = [
        {
            key: "Open" as AlertFilter,
            label: `Open (${openAlerts.length})`
        },
        {
            key: "Pending" as AlertFilter,
            label: `Pending (${pendingAlerts.length})`
        },
        {
            key: "Warning" as AlertFilter,
            label: `Warning (${warningAlerts.length})`
        },
        {
            key: "Critical" as AlertFilter,
            label: `Critical (${criticalAlerts.length})`
        },
        {
            key: "Closed" as AlertFilter,
            label: "Closed"
        },
        {
            key: "All" as AlertFilter,
            label: `All (${alerts.length})`
        }
    ];

    // -------------------------------------------------------------------------
    // Render
    // -------------------------------------------------------------------------

    return (
        <div className="page">

            <AppNav />

            {/* -------------------------------------------------------------
                Header
               ------------------------------------------------------------- */}

            <PageHeader title="Alerts">
                <LastUpdated value={lastUpdated} />
            </PageHeader>

            {/* -------------------------------------------------------------
                Summary Cards
               ------------------------------------------------------------- */}

            <div className="alerts-summary-grid">

                <DashboardCard
                    title="Open Alerts"
                    value={openAlerts.length}
                    statusClass="critical"
                />

                <DashboardCard
                    title="Critical Alerts"
                    value={criticalAlerts.length}
                    statusClass="critical"
                />

                <DashboardCard
                    title="Acknowledged"
                    value={acknowledgedAlerts.length}
                    statusClass="warning"
                />

                <DashboardCard
                    title="Suppressed"
                    value={suppressedAlerts.length}
                    statusClass="offline"
                />

                <DashboardCard
                    title="Closed Today"
                    value={closedToday.length}
                    statusClass="healthy"
                />

                <DashboardCard
                    title="Total Alerts"
                    value={alerts.length}
                />

            </div>

            {/* -------------------------------------------------------------
                Filters
               ------------------------------------------------------------- */}

            <div className="alerts-filters">

                {filterButtons.map(button => (

                    <button
                        key={button.key}
                        className={
                            filter === button.key
                                ? "range-button active"
                                : "range-button"
                        }
                        onClick={() =>
                            setFilter(
                                button.key
                            )
                        }
                    >
                        {button.label}
                    </button>

                ))}

            </div>

            {/* -------------------------------------------------------------
                Alert Table
               ------------------------------------------------------------- */}

            <table className="details-table">

                <thead>
                    <tr>
                        <th>Server</th>
                        <th>Rule</th>
                        <th>Severity</th>
                        <th>Status</th>
                        <th>Occurrences</th>
                        <th>Opened</th>
                    </tr>
                </thead>

                <tbody>

                    {filteredAlerts.map(alert => (

                        <tr
                            key={
                                alert.alertEventId
                            }
                        >

                            <td>
                                <Link
                                    to={`/servers/${alert.serverId}`}
                                >
                                    {alert.serverName}
                                </Link>
                            </td>

                            <td>
                                <Link
                                    to={`/alerts/${alert.alertEventId}`}
                                >
                                    {alert.ruleName}
                                </Link>
                            </td>

                            <td>
                                {alert.severity}
                            </td>

                            <td>
                                {alert.status}
                            </td>

                            <td>
                                {alert.occurrenceCount}
                            </td>

                            <td>
                                {new Date(
                                    alert.openedUtc
                                ).toLocaleString()}
                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

            {/* -------------------------------------------------------------
            Footer
            ------------------------------------------------------------- */}
            <Footer />

        </div>
    );
}