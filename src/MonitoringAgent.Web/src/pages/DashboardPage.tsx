/**
 * ============================================================================
 * Dashboard Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Provides a high-level operational overview of the monitored environment.
 *
 * Features:
 * - Fleet-wide resource utilization
 * - Server health summary
 * - Alert counts
 * - Worker/service status
 * - Top problem servers
 *
 * Notes:
 * Intended as the primary landing page for operators.
 * Refreshes automatically to provide near real-time status updates.
 * ============================================================================
 */

// Styles
import "./DashboardPage.css";

// React
import {
    useEffect,
    useState
} from "react";

import {
    Link
} from "react-router-dom";

// APIs
import { api } from "../api/api";

import {
    getServers
} from "../api/serverApi";

// Models
import type {
    DashboardTrendResponse
} from "../models/DashboardTrendResponse";

import type {
    ServerResponse
} from "../models/ServerResponse";

// Components
import DashboardCard from "../components/DashboardCard";
import ServerTable from "../components/ServerTable";
import LastUpdated from "../components/LastUpdated";
import LoadingIndicator from "../components/LoadingIndicator";
import ErrorMessage from "../components/ErrorMessage"

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
 * Dashboard card configuration.
 */
type DashboardCardConfig = {
    title: string;
    value: string | number;
    statusClass?: string;
};

/**
 * Dashboard Page
 *
 * High-level monitoring dashboard displaying:
 * - Fleet health metrics
 * - Resource utilization trends
 * - Alert counts
 * - Worker status
 * - Top problem servers
 * - Complete server inventory
 */
export default function DashboardPage() {

    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("Dashboard");

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [trend, setTrend] =
        useState<DashboardTrendResponse | null>(null);

    const [servers, setServers] =
        useState<ServerResponse[]>([]);

    const [lastUpdated, setLastUpdated] =
        useState(new Date());

    const [error, setError] =
        useState<string | null>(
            null);

    // -------------------------------------------------------------------------
    // Data Loading
    // -------------------------------------------------------------------------

    const loadDashboard = async () => {

        try {

            const [
                trendResponse,
                serverResponse
            ] = await Promise.all([
                api.get<DashboardTrendResponse>(
                    "/dashboard/trends"
                ),
                getServers()
            ]);

            setTrend(
                trendResponse.data
            );

            setServers(
                serverResponse
            );

            setLastUpdated(
                new Date()
            );
        }
        catch (error)
        {
            console.error(error);

            setError(
                "Unable to load dashboard data. Please verify the monitoring API is running."
            );
        }
    };

    // -------------------------------------------------------------------------
    // Auto Refresh
    // -------------------------------------------------------------------------

    useEffect(() => {

        loadDashboard();

        const interval =
            setInterval(
                loadDashboard,
                15000
            );

        return () =>
            clearInterval(
                interval
            );

    }, []);

    // -------------------------------------------------------------------------
    // Error State
    // -------------------------------------------------------------------------

    if (error)
    {
        return (
            <ErrorMessage
                message={error}
            />
        );
    }

    // -------------------------------------------------------------------------
    // Loading State
    // -------------------------------------------------------------------------

    if (!trend) {
        return <LoadingIndicator />;
    }

    // -------------------------------------------------------------------------
    // Dashboard Cards
    // -------------------------------------------------------------------------

    const dashboardCards: DashboardCardConfig[] = [
        {
            title: "Avg CPU",
            value: `${trend.cpuAverage.toFixed(1)}%`
        },
        {
            title: "Peak CPU",
            value: `${trend.cpuMaximum.toFixed(1)}%`
        },
        {
            title: "Avg Memory",
            value: `${trend.memoryAverage.toFixed(1)}%`
        },
        {
            title: "Peak Memory",
            value: `${trend.memoryMaximum.toFixed(1)}%`
        },
        {
            title: "Avg Disk",
            value: `${trend.diskAverage.toFixed(1)}%`
        },
        {
            title: "Peak Disk",
            value: `${trend.diskMaximum.toFixed(1)}%`
        },
        {
            title: "Healthy",
            value: trend.healthyServers,
            statusClass: "healthy"
        },
        {
            title: "Warning",
            value: trend.warningServers,
            statusClass: "warning"
        },
        {
            title: "Critical",
            value: trend.criticalServers,
            statusClass: "critical"
        },
        {
            title: "Offline",
            value: trend.offlineServers,
            statusClass: "offline"
        },
        {
            title: "Alerts",
            value: trend.totalAlertsOpened
        },
        {
            title: "Gateway RTT",
            value: `${trend.gatewayResponseAverageMs.toFixed(0)} ms`
        },
        {
            title: "Open Alerts",
            value: trend.openAlerts,
            statusClass: "warning"
        },
        {
            title: "Critical Alerts",
            value: trend.criticalAlerts,
            statusClass: "critical"
        },
        {
            title: "Workers Running",
            value: trend.runningWorkers,
            statusClass: "healthy"
        },
        {
            title: "Workers Stopped",
            value: trend.stoppedWorkers,
            statusClass: "critical"
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

            <PageHeader title="Dashboard">
                <LastUpdated value={lastUpdated} />
            </PageHeader>

            {/* -------------------------------------------------------------
                Summary Metrics
               ------------------------------------------------------------- */}

            <div className="dashboard-summary-grid">

                {dashboardCards.map(card => (

                    <DashboardCard
                        key={card.title}
                        title={card.title}
                        value={card.value}
                        statusClass={card.statusClass}
                    />

                ))}

            </div>

            {/* -------------------------------------------------------------
                Top Problem Servers
               ------------------------------------------------------------- */}

            <h2>
                Top Problem Servers
            </h2>

            <table className="details-table">

                <thead>
                    <tr>
                        <th>Server</th>
                        <th>Open Alerts</th>
                    </tr>
                </thead>

                <tbody>

                    {trend.topProblemServers.map(
                        server => (

                            <tr
                                key={
                                    server.serverId
                                }
                            >
                                <td>
                                    <Link
                                        to={`/servers/${server.serverId}`}
                                    >
                                        {server.serverName}
                                    </Link>
                                </td>

                                <td>
                                    {server.openAlertCount}
                                </td>

                            </tr>

                        )
                    )}

                </tbody>

            </table>

            {/* -------------------------------------------------------------
                Server Inventory
               ------------------------------------------------------------- */}

            <h2>
                Servers
            </h2>

            <ServerTable
                servers={servers}
            />
            
            {/* -------------------------------------------------------------
                Footer
               ------------------------------------------------------------- */}
            <Footer />

        </div>
    );
}