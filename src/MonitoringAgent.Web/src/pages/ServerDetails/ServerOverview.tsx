// React
import {
    useMemo
} from "react";

// Helpers
import {
    getLastSeenAge
} from "./ServerDetailsHelpers";

// Models
import type {
    ServerDetailsResponse
} from "../../models/ServerDetailsResponse";

import type {
    GatewaySnapshotResponse
} from "../../models/GatewaySnapshotResponse";

import type {
    IgnitionSnapshotResponse
} from "../../models/IgnitionSnapshotResponse";

// Components
import DashboardCard
    from "../../components/DashboardCard";

// Styles
import "./ServerOverview.css";

type Props = {
    server: ServerDetailsResponse;

    cpuTrend: number[];
    memoryTrend: number[];
    diskTrend: number[];

    cpuDelta: number;
    memoryDelta: number;
    diskDelta: number;

    gatewayHistory: GatewaySnapshotResponse[];
    ignitionHistory: IgnitionSnapshotResponse[];

    agentStatus: string;
    agentText: string;

    gatewayStatus: string;
    gatewayText: string;

    ignitionStatus: string;
    ignitionText: string;
};

/**
 * Server Overview
 *
 * Displays a high-level summary of server health and performance.
 *
 * Sections:
 * - Resource utilization cards
 * - Gateway performance
 * - Ignition performance
 * - Service status summary
 *
 * This serves as the primary dashboard section at the top
 * of the Server Details page.
 */
export function ServerOverview({
    server,
    cpuTrend,
    memoryTrend,
    diskTrend,
    cpuDelta,
    memoryDelta,
    diskDelta,
    gatewayHistory,
    ignitionHistory,
    agentStatus,
    agentText,
    gatewayStatus,
    gatewayText,
    ignitionStatus,
    ignitionText
}: Props) {

    // -------------------------------------------------------------------------
    // Derived Sparkline Data
    // -------------------------------------------------------------------------
    // Only the most recent samples are shown in the dashboard cards.
    // Memoizing prevents recalculation on unrelated component renders.
    // -------------------------------------------------------------------------

    const gatewaySparkline = useMemo(
        () =>
            gatewayHistory
                .slice(-50)
                .map(metric => metric.responseMs),
        [gatewayHistory]
    );

    const ignitionSparkline = useMemo(
        () =>
            ignitionHistory
                .slice(-50)
                .map(metric => metric.memoryMb),
        [ignitionHistory]
    );

    return (
        <>
            {/* -----------------------------------------------------------------
                Resource Summary Cards
               ----------------------------------------------------------------- */}

            <div className="overview-metrics-grid">

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
                    sparklineData={gatewaySparkline}
                    sparklineColor="#a855f7"
                />

                <DashboardCard
                    title="Ignition Memory"
                    value={`${server.ignition?.memoryMb ?? 0} MB`}
                    sparklineData={ignitionSparkline}
                    sparklineColor="#14b8a6"
                />

            </div>

            {/* -----------------------------------------------------------------
                Service Status Summary
               ----------------------------------------------------------------- */}

            <h2>Service Status</h2>

            <div className="service-status-grid">

                <DashboardCard
                    title="Agent"
                    value={agentText}
                    statusClass={agentStatus}
                    subtitle={
                        `Last seen ${getLastSeenAge(
                            server.lastSeenUtc
                        )} ago`
                    }
                />

                <DashboardCard
                    title="Gateway"
                    value={gatewayText}
                    statusClass={gatewayStatus}
                    subtitle={
                        `${server.gateway?.responseMs ?? 0} ms`
                    }
                />

                <DashboardCard
                    title="Ignition"
                    value={ignitionText}
                    statusClass={ignitionStatus}
                    subtitle={
                        `${server.ignition?.memoryMb ?? 0} MB`
                    }
                />

            </div>
        </>
    );
}