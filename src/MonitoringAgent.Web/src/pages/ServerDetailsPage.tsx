// Import CSS
import "./ServerDetailsPage.css";

// Imports
import {
    useEffect,
    useState,
    useMemo
}
from "react";
import { Link } from "react-router-dom";

import {
    useParams
}
from "react-router-dom";

// Import API Calls

import {
    getServer,
    getAlerts,
    getHostHistory,
    getGatewayHistory,
    getIgnitionHistory,
    unsuppressAlert,
    suppressAlert,
    unacknowledgeAlert,
    acknowledgeAlert
}
from "../api/serverApi";

// Import Models/Responses

import type {
    ServerDetailsResponse
}
from "../models/ServerDetailsResponse";

import type {
    ServerHistoryPointResponse
}
from "../models/ServerHistoryPointResponse";

import type {
    AlertHistoryResponse
}
from "../models/AlertHistoryResponse";

// Components
import MetricChart
    from "../components/MetricChart";

import MultiMetricChart
    from "../components/MultiMetricChart";

import ChartCard from "../components/ChartCard"

import DashboardCard
    from "../components/DashboardCard";

import StatusBadge 
    from "../components/StatusBadge";

import AlertStatusBadge 
    from "../components/AlertStatusBadge";

export default function ServerDetailsPage() {

    const { id } = useParams();

    const [server, setServer] =
        useState<
            ServerDetailsResponse | null
        >(null);

    const [history, setHostHistory] =
        useState<
            ServerHistoryPointResponse[]
        >([]);

    const [gatewayHistory, setGatewayHistory] =
        useState<any[]>([]);

    const [ignitionHistory, setIgnitionHistory] =
        useState<any[]>([]);

    const [hours, setHours] =
    useState(24);

    const [recentAlerts, setRecentAlerts] =
        useState<
            AlertHistoryResponse[]
        >([]);

    // Auto Refresh
    const [lastUpdated, setLastUpdated] =
    useState(new Date());

    const cpuTrend =
        useMemo(
            () =>
                history
                    .slice(-50)
                    .map(x => x.cpuPercent),
            [history]);

    const memoryTrend =
        useMemo(
            () =>
                history
                    .slice(-50)
                    .map(x => x.memoryPercent),
            [history]);

    const diskTrend =
        useMemo(
            () =>
                history
                    .slice(-50)
                    .map(x => x.diskPercentUsed),
            [history]);

    const intervalMinutes =
        getIntervalMinutes(hours);

    // Refactor Loading
    const loadData = () => {

        if (!id)
            return;

        Promise.all([
            getServer(Number(id)),
            getHostHistory(Number(id), Number(hours), Number(intervalMinutes)),
            getGatewayHistory(Number(id), Number(hours), Number(intervalMinutes)),
            getIgnitionHistory(Number(id), Number(hours), Number(intervalMinutes)),
            getAlerts(Number(id))
        ])
        .then(([server,
            host,
            gateway,
            ignition,
            alerts]) =>
        {
            setServer(server);
            setHostHistory(host);
            setGatewayHistory(gateway);
            setIgnitionHistory(ignition);
            setRecentAlerts(alerts);

            setLastUpdated(
                new Date());
        });
    };
    
    useEffect(() => {

    const loadCurrentSnapshot = () => {

        if (!id)
            return;

        getServer(Number(id))
            .then(server =>
            {
                setServer(server);

                setLastUpdated(
                new Date());
            });

        setLastUpdated(
            new Date());
    };

    loadData();

    const timer =
        setInterval(
            loadCurrentSnapshot,
            30000);

    return () =>
        clearInterval(timer);

    }, [id, hours]
    );

const chartData =
    useMemo(
        () =>
            history.map(x => ({
                timestamp:
                    new Date(
                        x.timestampUtc)
                        .getTime(),

                cpuPercent:
                    x.cpuPercent,

                memoryPercent:
                    x.memoryPercent,

                diskPercentUsed:
                    x.diskPercentUsed,

                diskReadsPerSec:
                    x.diskReadsPerSec,

                diskWritesPerSec:
                    x.diskWritesPerSec,

                avgDiskQueueLength:
                    x.avgDiskQueueLength,

                diskReadLatencyMs:
                    x.diskReadLatencyMs,

                diskWriteLatencyMs:
                    x.diskWriteLatencyMs,

                networkReceived:
                    x.networkBytesReceivedPerSec /
                    1024 /
                    1024,

                networkSent:
                    x.networkBytesSentPerSec /
                    1024 /
                    1024,

                gatewayResponseMs:
                    x.gatewayResponseMs
            })),
        [history]);

    const alertMarkers =
    useMemo(
        () =>
            recentAlerts.map(alert => ({
                id:
                    alert.alertEventId,

                timestamp:
                    new Date(
                        alert.firstTriggeredUtc ??
                        alert.openedUtc)
                        .getTime(),

                label:
                    alert.ruleName
            })),
        [recentAlerts]);
  
    const gatewayChartData =
        useMemo(
            () =>
                gatewayHistory.map(x => ({
                    time:
                        formatTimestamp(
                            x.snapshotUtc),

                    responseMs:
                        x.responseMs
                })),
            [gatewayHistory, hours]);

    const ignitionChartData =
        useMemo(
            () =>
                ignitionHistory.map(x => ({
                    time:
                        formatTimestamp(
                            x.snapshotUtc),

                    memoryMb:
                        x.memoryMb,

                    cpuPercent:
                        x.cpuPercent,

                    threadCount:
                        x.threadCount,

                    handleCount:
                        x.handleCountrecentAlerts
                })),
            [ignitionHistory, hours]);

    const openAlerts =
        recentAlerts.filter(
            alert => alert.status !== "Closed");

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

    const recentEvents = useMemo(() =>
    {
        const events = [];

        for (const alert of recentAlerts)
        {
            events.push({
                id: `${alert.alertEventId}-open`,
                type: "down",
                timestamp:
                    alert.firstTriggeredUtc ??
                    alert.openedUtc,
                text:
                    alert.ruleName
            });

            if (alert.closedUtc)
            {
                events.push({
                    id: `${alert.alertEventId}-close`,
                    type: "up",
                    timestamp:
                        alert.closedUtc,
                    text:
                        alert.ruleName.includes("Down")
                            ? alert.ruleName.replace(
                                "Down",
                                "Restored")
                            : `${alert.ruleName} Recovered`
                });
            }
        }

        return events
            .sort(
                (a, b) =>
                    new Date(b.timestamp).getTime() -
                    new Date(a.timestamp).getTime())
            .slice(0, 20);
    }, [recentAlerts]);

    if (!server)
    {
        return <div>Loading...</div>;
    }

    const gatewayStatus =
        !server.gateway?.reachable
            ? "critical"
            : (server.gateway.responseMs ?? 0) > 250
                ? "warning"
                : "healthy";

    const gatewayText =
        !server.gateway?.reachable
            ? "Offline"
            : (server.gateway.responseMs ?? 0) > 500
                ? "Slow"
                : "Healthy";

    const ignitionStatus =
        !server.ignition?.serviceRunning
            ? "critical"
            : (server.ignition.memoryMb ?? 0) > 4096
                ? "warning"
                : "healthy";

    const ignitionText =
        !server.ignition?.serviceRunning
            ? "Stopped"
            : (server.ignition.memoryMb ?? 0) > 4096
                ? "High Memory"
                : "Running";

    const minutesSinceLastSeen =
        Math.floor(
            (
                Date.now() -
                new Date(
                    server.lastSeenUtc)
                    .getTime()
            ) / 60000);

    const agentStatus =
        minutesSinceLastSeen > 5
            ? "critical"
            : minutesSinceLastSeen > 2
                ? "warning"
                : "healthy";

    const agentText =
        minutesSinceLastSeen > 5
            ? "Offline"
            : minutesSinceLastSeen > 2
                ? "Delayed"
                : "Online";

    return (
        <div
            style={{
                padding: "24px"
            }}
        >
        <div
            style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginBottom: "20px"
            }}
        >
            <div
                style={{
                    display: "flex",
                    alignItems: "center",
                    gap: "12px"
                }}
            >
                <h1
                    style={{
                        margin: 0
                    }}
                >
                    {server.serverName}
                </h1>

                <StatusBadge
                    status={server.status}
                />
            </div>

            <div
                style={{
                    color: "#94a3b8"
                }}
            >
                Last Updated:
                {" "}
                {lastUpdated.toLocaleTimeString()}
            </div>
        </div>
        <div
            style={{
                color: "#94a3b8",
                marginTop: "4px",
                marginBottom: "20px"
            }}
        >
            {server.operatingSystem}
            {" • "}
            {server.processorCount} cores
            {" • "}
            {(server.totalMemoryMb / 1024).toFixed(1)} GB RAM
        </div>
        <Link
            to="/"
            style={{
                display: "inline-block",
                marginBottom: "20px"
            }}
        >
            ← Dashboard
        </Link>

            <div
                style={{
                    display: "grid",
                    gridTemplateColumns:
                        "repeat(auto-fit, minmax(250px, 1fr))",
                    gap: "20px"
                }}
            >
                <DashboardCard
                    title="CPU"
                    value={`${server.host.cpuPercent}%`}
                    trend={cpuDelta}
                    sparklineData={cpuTrend}
                    sparklineColor="#22c55e"
                />

                <DashboardCard
                    title="Memory"
                    value={`${server.host.memoryPercent}%`}
                    trend={memoryDelta}
                    sparklineData={memoryTrend}
                    sparklineColor="#3b82f6"
                />

                <DashboardCard
                    title="Disk"
                    value={`${server.host.diskPercentUsed}%`}
                    trend={diskDelta}
                    sparklineData={diskTrend}
                    sparklineColor="#f59e0b"
                />

                <DashboardCard
                    title="Gateway RTT"
                    value={`${server.gateway?.responseMs ?? 0} ms`}
                    
                    sparklineData={
                        gatewayHistory
                            .slice(-50)
                            .map(x => x.responseMs)
                    }
                    sparklineColor="#a855f7"
                />

                <DashboardCard
                    title="Ignition Memory"
                    value={`${server.ignition?.memoryMb ?? 0} MB`}
                    sparklineData={
                        ignitionHistory
                            .slice(-50)
                            .map(x => x.memoryMb)
                    }
                    sparklineColor="#14b8a6"
                />

            </div>
            <h2>Service Status</h2>
            <div className="service-status-grid">

                <DashboardCard
                    title="Agent"
                    value={agentText}
                    statusClass={agentStatus}
                    subtitle={`Last seen ${getLastSeenAge(
                        server.lastSeenUtc)} ago`}
                />

                <DashboardCard
                    title="Gateway"
                    value={gatewayText}
                    statusClass={gatewayStatus}
                    subtitle={`${server.gateway?.responseMs ?? 0} ms`}
                />

                <DashboardCard
                    title="Ignition"
                    value={ignitionText}
                    statusClass={ignitionStatus}
                    subtitle={`${server.ignition?.memoryMb ?? 0} MB`}
                />

            </div>
            <h2>System Information</h2>

            <table className="details-table">
                <tbody>
                    <tr>
                        <td>Status</td>
                        <td>
                            <StatusBadge
                                status={server.status}
                            />
                        </td>
                    </tr>
                    <tr>
                        <td>Domain</td>
                        <td>{server.domainName}</td>
                    </tr>

                    <tr>
                        <td>Agent Version</td>
                        <td>{server.agentVersion}</td>
                    </tr>

                    <tr>
                        <td>Operating System</td>
                        <td>{server.operatingSystem}</td>
                    </tr>

                    <tr>
                        <td>OS Version</td>
                        <td>{server.operatingSystemVersion}</td>
                    </tr>

                    <tr>
                        <td>CPU Cores</td>
                        <td>{server.processorCount}</td>
                    </tr>

                    <tr>
                        <td>Total Memory</td>
                        <td>{server.totalMemoryMb?.toLocaleString()} MB</td>
                    </tr>

                    <tr>
                        <td>Last Seen</td>
                        <td>
                            {new Date(
                                server.lastSeenUtc)
                                .toLocaleString()}
                        </td>
                    </tr>
                </tbody>
            </table>
            <h2>Host Metrics</h2>
            <table className="details-table">
                <tbody>
                    <tr>
                        <td>CPU Usage</td>
                        <td>{server.host.cpuPercent}%</td>
                    </tr>

                    <tr>
                        <td>Memory Usage</td>
                        <td>{server.host.memoryPercent}%</td>
                    </tr>

                    <tr>
                        <td>Disk Usage</td>
                        <td>{server.host.diskPercentUsed}%</td>
                    </tr>

                    <tr>
                        <td>Available Memory</td>
                        <td>
                            {server.host.availableMemoryMb
                                ?.toLocaleString()} MB
                        </td>
                    </tr>

                    <tr>
                        <td>System Uptime</td>
                        <td>
                            {formatMinutes(server.host.systemUptimeMinutes)}
                        </td>
                    </tr>
                </tbody>
            </table>
            <h2>Ignition</h2>
            <table className="details-table">
                <tbody>

                    <tr>
                        <td>Process Running</td>
                        <td>
                            {server.ignition?.processRunning
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>Service Running</td>
                        <td>
                            {server.ignition?.serviceRunning
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>Version</td>
                        <td>
                            {server.ignition?.ignitionVersion}
                        </td>
                    </tr>

                    <tr>
                        <td>Memory</td>
                        <td>
                            {server.ignition?.memoryMb}
                            {" "}MB
                        </td>
                    </tr>

                </tbody>
            </table>
            <h2>Gateway</h2>
            <table className="details-table">
                <tbody>
                    <tr>
                        <td>Reachable</td>
                        <td>
                            {server.gateway?.reachable
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>HTTP Status</td>
                        <td>
                            {server.gateway?.httpStatusCode}
                        </td>
                    </tr>

                    <tr>
                        <td>Response Time</td>
                        <td>
                            {server.gateway?.responseMs} ms
                        </td>
                    </tr>
                </tbody>
            </table>
           <h2>Open Alerts</h2>
            <div className="open-alerts">
            {
                openAlerts.map(alert =>
                {
                    const cardClass =
                        alert.status === "Acknowledged"
                            ? "open-alert-card acknowledged"
                            : alert.status === "Suppressed"
                                ? "open-alert-card suppressed"
                                : "open-alert-card open";

                    return (
                        <div
                            key={alert.alertEventId}
                            className={cardClass}
                            title={alert.message}
                        >
                            <AlertStatusBadge
                                status={alert.status}
                            />

                            <div>
                                <strong>
                                    {alert.ruleName}
                                </strong>

                                <div>
                                    Open for{" "}
                                    {formatDuration(
                                        alert.firstTriggeredUtc ??
                                        alert.openedUtc,
                                        alert.closedUtc
                                    )}
                                </div>

                                <div className="open-alert-occurrences">
                                    {alert.occurrenceCount} occurrences
                                </div>
                            </div>
                            <div className="open-alert-actions">

                                {
                                    alert.status === "Open" &&
                                    (
                                        <>
                                            <button
                                                onClick={() =>
                                                    handleAcknowledge(
                                                        alert.alertEventId)}
                                            >
                                                Acknowledge
                                            </button>

                                            <button
                                                onClick={() =>
                                                    handleSuppress(
                                                        alert.alertEventId)}
                                            >
                                                Suppress
                                            </button>
                                        </>
                                    )
                                }

                                {
                                    alert.status === "Acknowledged" &&
                                    (
                                        <>
                                            <button
                                                onClick={() =>
                                                    handleUnacknowledge(
                                                        alert.alertEventId)}
                                            >
                                                Unacknowledge
                                            </button>

                                            <button
                                                onClick={() =>
                                                    handleSuppress(
                                                        alert.alertEventId)}
                                            >
                                                Suppress
                                            </button>
                                        </>
                                    )
                                }

                                {
                                    alert.status === "Suppressed" &&
                                    (
                                        <>
                                            <button
                                                onClick={() =>
                                                    handleUnsuppress(
                                                        alert.alertEventId)}
                                            >
                                                Unsuppress
                                            </button>
                                        </>
                                    )
                                }

                            </div>
                        </div>
                    );
                })
            }
            </div>
            <h2>Recent Events</h2>

            <div className="event-feed">
            {
                recentEvents.map(event => (
                    <div
                        key={event.id}
                        className="event-item"
                    >
                        <span
                            className={
                                event.type === "down"
                                    ? "event-icon down"
                                    : "event-icon up"
                            }
                        >
                            {
                                event.type === "down"
                                    ? "🟥"
                                    : "🟩"
                            }
                        </span>

                        <span className="event-time">
                            {formatEventTimestamp(event.timestamp)}
                        </span>

                        <span className="event-message">
                            {event.text}
                        </span>
                    </div>
                ))
            }
            </div>
            <h2>Recent Alerts</h2>
            {
                recentAlerts.length === 0
                    ? (
                        <div className="empty-state">
                            No recent alerts.
                        </div>
                    )
                    : (
                        <table className="alert-table">

                            <thead>
                                <tr>
                                    <th className="status-column">Status</th>
                                    <th className="rule-column">Rule</th>
                                    <th className="occurrences-column">Occurrences</th>
                                    <th className="date-column">Triggered</th>
                                    <th className="date-column">Opened</th>
                                    <th className="date-column">Closed</th>
                                    <th>Duration</th>
                                </tr>
                            </thead>

                            <tbody>

                                {
                                    recentAlerts.map(alert => (

                                        <tr title={alert.message} key={alert.alertEventId}>

                                            <td>
                                                <AlertStatusBadge
                                                    status={alert.status}
                                                />
                                            </td>

                                            <td>
                                                {alert.ruleName}
                                            </td>

                                            <td>
                                                <span className="occurrence-badge">
                                                    {alert.occurrenceCount}
                                                </span>
                                            </td>
                                            
                                            <td>
                                                {formatDate(alert.firstTriggeredUtc)}
                                            </td>
                                            
                                            <td>
                                                {formatDate(alert.openedUtc)}
                                            </td>

                                            <td>
                                                {formatDate(alert.closedUtc)}
                                            </td>

                                            <td>
                                                {formatDuration(
                                                    alert.firstTriggeredUtc ??
                                                    alert.openedUtc,
                                                    alert.closedUtc
                                                )}
                                            </td>

                                        </tr>

                                    ))
                                }

                            </tbody>

                        </table>
                    )
            }
            <h2>Performance History</h2>
            <div
                style={{
                    display: "flex",
                    gap: "10px",
                    marginBottom: "20px"
                }}
            >
                <button
                    className={
                        hours === 1
                            ? "range-button active"
                            : "range-button"
                    }
                    onClick={() => setHours(1)}
                >
                    1 Hour
                </button>

                <button
                    className={
                        hours === 6
                            ? "range-button active"
                            : "range-button"
                    }
                    onClick={() => setHours(6)}
                >
                    6 Hours
                </button>

                <button
                    className={
                        hours === 24
                            ? "range-button active"
                            : "range-button"
                    }
                    onClick={() => setHours(24)}
                >
                    24 Hours
                </button>

                <button
                    className={
                        hours === 168
                            ? "range-button active"
                            : "range-button"
                    }
                    onClick={() => setHours(168)}
                >
                    7 Days
                </button>
            </div>
            <ChartCard title="Host">
                <MetricChart
                    title="CPU Usage"
                    data={chartData}
                    dataKey="cpuPercent"
                    color="#22c55e"
                    yDomain={[0, 100]}
                    alertMarkers={alertMarkers}
                />

                <MetricChart
                    title="Memory Usage"
                    data={chartData}
                    dataKey="memoryPercent"
                    color="#3b82f6"
                    yDomain={[0, 100]}
                    alertMarkers={alertMarkers}
                />

                <MetricChart
                    title="Disk Usage"
                    data={chartData}
                    dataKey="diskPercentUsed"
                    color="#f97316"
                    yDomain={[0, 100]}
                    alertMarkers={alertMarkers}
                />
            </ChartCard>
            <ChartCard title="Network">
                <MultiMetricChart
                    title="Network Throughput"
                    data={chartData}
                    lines={[
                        {
                            key: "networkReceived",
                            color: "#8b5cf6",
                            name: "Receive MB/sec"
                        },
                        {
                            key: "networkSent",
                            color: "#ec4899",
                            name: "Send MB/sec"
                        }
                    ]}
                />
            </ChartCard>
            <ChartCard title="Storage">
                <MultiMetricChart
                    title="Disk Activity"
                    data={chartData}
                    lines={[
                        {
                            key: "diskReadsPerSec",
                            color: "#06b6d4",
                            name: "Reads/sec"
                        },
                        {
                            key: "diskWritesPerSec",
                            color: "#f97316",
                            name: "Writes/sec"
                        }
                    ]}
                />

                <MultiMetricChart
                    title="Disk Latency"
                    data={chartData}
                    lines={[
                        {
                            key: "diskReadLatencyMs",
                            color: "#22c55e",
                            name: "Read Latency"
                        },
                        {
                            key: "diskWriteLatencyMs",
                            color: "#ef4444",
                            name: "Write Latency"
                        }
                    ]}
                />

                <MetricChart
                    title="Disk Queue Length"
                    data={chartData}
                    dataKey="avgDiskQueueLength"
                    color="#facc15"
                />
            </ChartCard>
            <ChartCard title="Ignition">
                <MetricChart
                    title="Ignition Memory"
                    data={ignitionChartData}
                    dataKey="memoryMb"
                    color="#14b8a6"
                />

            <MetricChart
                    title="Ignition CPU"
                    data={ignitionChartData}
                    dataKey="cpuPercent"
                    color="#fb7185"
                />

                <MetricChart
                    title="Ignition Threads"
                    data={ignitionChartData}
                    dataKey="threadCount"
                    color="#f59e0b"
                />

                <MetricChart
                    title="Ignition Handles"
                    data={ignitionChartData}
                    dataKey="handleCount"
                    color="#ef4444"
                />
             </ChartCard>   
            <ChartCard title="Gateway">
                <MetricChart
                    title="Gateway Response Time"
                    data={gatewayChartData}
                    dataKey="responseMs"
                    color="#a855f7"
                />
            </ChartCard>

        </div>
        
    );

    function formatMinutes(minutes: number)
    {
        const hours =
            Math.floor(minutes / 60);

        const remaining =
            minutes % 60;

        return `${hours}h ${remaining}m`;
    }

    function getIntervalMinutes(hours: number)
    {
        if (hours <= 1)
            return 1;

        if (hours <= 6)
            return 5;

        if (hours <= 24)
            return 15;

        return 60;
    }
    function formatTimestamp(timestamp: string)
    {

        const date =
            new Date(timestamp);

        if (date.getFullYear() < 2000)
            return "-";

        if (hours <= 24)
            return date.toLocaleTimeString();

        return date.toLocaleDateString();
    };
    function formatDate(
        value?: string)
    {
        if (!value)
            return "-";

        const date =
            new Date(value);

        if (date.getFullYear() < 2000)
            return "-";

        return date.toLocaleString();
    }
    function formatDuration(
        start: string,
        end?: string)
    {
        const startDate =
            new Date(start);

        if (startDate.getFullYear() < 2000)
            return "-";

        const endDate =
            end
                ? new Date(end)
                : new Date();

        const ms =
            endDate.getTime() -
            startDate.getTime();

        const minutes =
            Math.floor(ms / 60000);

        const hours =
            Math.floor(minutes / 60);

        const remainingMinutes =
            minutes % 60;

        if (hours > 0)
        {
            return `${hours}h ${remainingMinutes}m`;
        }

        return `${minutes}m`;
    }

    function getLastSeenAge(timestamp: string)
    {
        const minutes =
            Math.floor(
                (Date.now() -
                new Date(timestamp).getTime())
                / 60000);

        if (minutes < 1)
            return "<1m";

        if (minutes < 60)
            return `${minutes}m`;

        const hours =
            Math.floor(minutes / 60);

        return `${hours}h`;
    }

    function formatEventTimestamp(
        timestamp: string)
    {
        return new Date(timestamp)
            .toLocaleString();
    }
    
    async function handleAcknowledge(
        alertId: number)
    {
        await acknowledgeAlert(
            alertId);

        await loadData();
    }
    async function handleUnacknowledge(
        alertId: number)
    {
        await unacknowledgeAlert(
            alertId);

        await loadData();
    }
    async function handleSuppress(
        alertId: number)
    {
        await suppressAlert(
            alertId,
            0.01);

        await loadData();
    }
    async function handleUnsuppress(
        alertId: number)
    {
        await unsuppressAlert(
            alertId);

        await loadData();
    }
}
