/**
 * ============================================================================
 * Server Details Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Primary troubleshooting and diagnostic view for a monitored server.
 *
 * Features:
 * - Current host metrics
 * - Gateway metrics
 * - Ignition metrics
 * - Open alerts
 * - Alert history
 * - Event timeline
 * - Historical performance charts
 *
 * Notes:
 * Historical data is sampled based on the selected time range.
 * Current snapshot information refreshes automatically.
 * ============================================================================
 */

// Styles
import "./ServerDetailsPage.css";

// React
import {
    useEffect,
    useState,
    useMemo
} from "react";

import {
    useParams
} from "react-router-dom";

// APIs
import {
    getServer,
    getAlerts,
    getHostHistory,
    getGatewayHistory,
    getIgnitionHistory
} from "../api/serverApi";

// Helpers
import {
    getIntervalMinutes
} from "./ServerDetails/ServerDetailsHelpers";

import {
    buildHostChartData,
    buildGatewayChartData,
    buildIgnitionChartData,
    buildAlertMarkers
} from "./ServerDetails/ServerDetailsChartHelpers";

import {
    getGatewayStatus,
    getGatewayText,
    getIgnitionStatus,
    getIgnitionText,
    getAgentStatus,
    getAgentText
} from "./ServerDetails/ServerDetailsStatusHelpers";

// Models
import type {
    ServerDetailsResponse
} from "../models/ServerDetailsResponse";

import type {
    ServerHistoryPointResponse
} from "../models/ServerHistoryPointResponse";

import type {
    AlertHistoryResponse
} from "../models/AlertHistoryResponse";

import type {
    EventFeedItem
} from "../models/EventFeedItem";

import type {
    GatewaySnapshotResponse
} from "../models/GatewaySnapshotResponse";

import type {
    IgnitionSnapshotResponse
} from "../models/IgnitionSnapshotResponse";

// Sections
import { Header } from "./ServerDetails/Header";
import { ServerOverview } from "./ServerDetails/ServerOverview";
import { InfrastructurePanel } from "./ServerDetails/InfrastructurePanel";
import { OpenAlertsPanel } from "./ServerDetails/OpenAlertsPanel";
import { RecentEventsPanel } from "./ServerDetails/RecentEventsPanel";
import { RecentAlertsPanel } from "./ServerDetails/RecentAlertsPanel";
import { PerformanceHistory } from "./ServerDetails/PerformanceHistory";

// Navigation
import AppNav 
    from "../nav/AppNav";
import Footer
    from "../nav/Footer";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

// Components
import LoadingIndicator
from "../components/LoadingIndicator";

/**
 * Server Details Page
 *
 * Displays detailed operational information for a monitored server,
 * including:
 *
 * - Current health status
 * - Resource utilization
 * - Gateway and Ignition metrics
 * - Active alerts
 * - Alert history
 * - Event timeline
 * - Historical performance charts
 */
export default function ServerDetailsPage() {

    // -------------------------------------------------------------------------
    // Route Parameters
    // -------------------------------------------------------------------------

    const { id } = useParams();

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [server, setServer] =
        useState<ServerDetailsResponse | null>(null);

    const [history, setHostHistory] =
        useState<ServerHistoryPointResponse[]>([]);

    const [gatewayHistory, setGatewayHistory] =
        useState<GatewaySnapshotResponse[]>([]);

    const [ignitionHistory, setIgnitionHistory] =
        useState<IgnitionSnapshotResponse[]>([]);

    const [recentAlerts, setRecentAlerts] =
        useState<AlertHistoryResponse[]>([]);

    const [hours, setHours] =
        useState(24);

    const [lastUpdated, setLastUpdated] =
        useState(new Date());

    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle(
    server
        ? server.serverName
        : "Server Details");

    // -------------------------------------------------------------------------
    // Derived Trends
    // -------------------------------------------------------------------------

    const cpuTrend = useMemo(
        () =>
            history
                .slice(-50)
                .map(x => x.cpuPercent),
        [history]
    );

    const memoryTrend = useMemo(
        () =>
            history
                .slice(-50)
                .map(x => x.memoryPercent),
        [history]
    );

    const diskTrend = useMemo(
        () =>
            history
                .slice(-50)
                .map(x => x.diskPercentUsed),
        [history]
    );

    const intervalMinutes =
        getIntervalMinutes(hours);

    // -------------------------------------------------------------------------
    // Data Loading
    // -------------------------------------------------------------------------

    const loadData = async () => {

        if (!id) {
            return;
        }

        Promise.all([
            getServer(Number(id)),
            getHostHistory(
                Number(id),
                Number(hours),
                Number(intervalMinutes)
            ),
            getGatewayHistory(
                Number(id),
                Number(hours),
                Number(intervalMinutes)
            ),
            getIgnitionHistory(
                Number(id),
                Number(hours),
                Number(intervalMinutes)
            ),
            getAlerts(Number(id))
        ])
        .then(([
            server,
            host,
            gateway,
            ignition,
            alerts
        ]) => {

            setServer(server);
            setHostHistory(host);
            setGatewayHistory(gateway);
            setIgnitionHistory(ignition);
            setRecentAlerts(alerts);

            setLastUpdated(new Date());
        });
    };

    // -------------------------------------------------------------------------
    // Auto Refresh
    // -------------------------------------------------------------------------

    useEffect(() => {

        const loadCurrentSnapshot = () => {

            if (!id) {
                return;
            }

            getServer(Number(id))
                .then(server => {

                    setServer(server);

                    setLastUpdated(
                        new Date()
                    );
                });
        };

        loadData();

        const timer =
            setInterval(
                loadCurrentSnapshot,
                30000
            );

        return () =>
            clearInterval(timer);

    }, [id, hours]);

    // -------------------------------------------------------------------------
    // Chart Data
    // -------------------------------------------------------------------------

    const chartData = useMemo(
        () =>
            buildHostChartData(history),
        [history]
    );

    const gatewayChartData = useMemo(
        () =>
            buildGatewayChartData(
                gatewayHistory
            ),
        [gatewayHistory]
    );

    const ignitionChartData = useMemo(
        () =>
            buildIgnitionChartData(
                ignitionHistory
            ),
        [ignitionHistory]
    );

    const alertMarkers = useMemo(
        () =>
            buildAlertMarkers(
                recentAlerts
            ),
        [recentAlerts]
    );

    // -------------------------------------------------------------------------
    // Derived Alert Data
    // -------------------------------------------------------------------------

    const openAlerts =
        recentAlerts.filter(
            alert => alert.status !== "Closed"
        );

    // -------------------------------------------------------------------------
    // Resource Trend Deltas
    // -------------------------------------------------------------------------

    const cpuDelta =
        history.length >= 2
            ? history.at(-1)!.cpuPercent -
              history.at(-2)!.cpuPercent
            : 0;

    const memoryDelta =
        history.length >= 2
            ? history.at(-1)!.memoryPercent -
              history.at(-2)!.memoryPercent
            : 0;

    const diskDelta =
        history.length >= 2
            ? history.at(-1)!.diskPercentUsed -
              history.at(-2)!.diskPercentUsed
            : 0;

    // -------------------------------------------------------------------------
    // Event Feed
    // -------------------------------------------------------------------------

    const recentEvents = useMemo(
        (): EventFeedItem[] => {

            const events: EventFeedItem[] = [];

            for (const alert of recentAlerts) {

                events.push({
                    id: `${alert.alertEventId}-open`,
                    type: "down",
                    timestamp:
                        alert.firstTriggeredUtc ??
                        alert.openedUtc,
                    text: alert.ruleName
                });

                if (alert.closedUtc) {

                    events.push({
                        id: `${alert.alertEventId}-close`,
                        type: "up",
                        timestamp: alert.closedUtc,
                        text:
                            alert.ruleName.includes("Down")
                                ? alert.ruleName.replace(
                                    "Down",
                                    "Restored"
                                )
                                : `${alert.ruleName} Recovered`
                    });
                }
            }

            return events
                .sort(
                    (a, b) =>
                        new Date(b.timestamp).getTime() -
                        new Date(a.timestamp).getTime()
                )
                .slice(0, 20);

        },
        [recentAlerts]
    );

    // -------------------------------------------------------------------------
    // Loading State
    // -------------------------------------------------------------------------

    if (!server) {
        return <LoadingIndicator />;
    }

    // -------------------------------------------------------------------------
    // Service Status
    // -------------------------------------------------------------------------

    const gatewayStatus =
        getGatewayStatus(server);

    const gatewayText =
        getGatewayText(server);

    const ignitionStatus =
        getIgnitionStatus(server);

    const ignitionText =
        getIgnitionText(server);

    const agentStatus =
        getAgentStatus(server);

    const agentText =
        getAgentText(server);

    // -------------------------------------------------------------------------
    // Render
    // -------------------------------------------------------------------------

    return (
        <div className="page">

            <AppNav />

            <Header
                server={server}
                lastUpdated={lastUpdated}
            />

            <ServerOverview
                server={server}
                cpuTrend={cpuTrend}
                memoryTrend={memoryTrend}
                diskTrend={diskTrend}
                cpuDelta={cpuDelta}
                memoryDelta={memoryDelta}
                diskDelta={diskDelta}
                gatewayHistory={gatewayHistory}
                ignitionHistory={ignitionHistory}
                agentStatus={agentStatus}
                agentText={agentText}
                gatewayStatus={gatewayStatus}
                gatewayText={gatewayText}
                ignitionStatus={ignitionStatus}
                ignitionText={ignitionText}
            />

            <InfrastructurePanel
                server={server}
            />

            <OpenAlertsPanel
                openAlerts={openAlerts}
                loadData={loadData}
            />

            <RecentAlertsPanel
                recentAlerts={recentAlerts}
            />

            <RecentEventsPanel
                recentEvents={recentEvents}
            />

            <PerformanceHistory
                hours={hours}
                setHours={setHours}
                chartData={chartData}
                gatewayChartData={gatewayChartData}
                ignitionChartData={ignitionChartData}
                alertMarkers={alertMarkers}
            />

            {/* -------------------------------------------------------------
            Footer
            ------------------------------------------------------------- */}
            <Footer />

        </div>
    );
}