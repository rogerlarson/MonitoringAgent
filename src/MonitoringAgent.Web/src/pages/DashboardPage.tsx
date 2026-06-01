import { useEffect, useState } from "react";
import { api } from "../api/api";
import DashboardCard from "../components/DashboardCard";
import { getServers } from "../api/serverApi";
import ServerTable from "../components/ServerTable";
import type { ServerResponse } from "../models/ServerResponse";

function DashboardPage() {
    const [trend, setTrend] = useState<any>(null);
    const [servers, setServers] = useState<ServerResponse[]>([]);

    useEffect(() => {
        api.get("/dashboard/trends")
            .then(response => {
                setTrend(response.data);
            });
        getServers()
          .then(setServers)
          .catch(console.error);
    }, []);

    if (!trend) {
        return <div>Loading...</div>;
    }

    return (
            <div
                style={{
                    padding: "24px",
                    width: "100%",
                    boxSizing: "border-box"
                }}
            >
            <h1
                style={{
                    marginTop: 0,
                    marginBottom: "24px"
                }}
            >
                Monitoring Dashboard
            </h1>

            <div
                style={{
                    display: "grid",
                    gridTemplateColumns:
                        "repeat(auto-fit, minmax(220px, 1fr))",
                    gap: "20px",
                    marginBottom: "30px"
                }}
            >
               <DashboardCard
                    title="Avg CPU"
                    value={`${trend.cpuAverage.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Peak CPU"
                    value={`${trend.cpuMaximum.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Avg Memory"
                    value={`${trend.memoryAverage.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Peak Memory"
                    value={`${trend.memoryMaximum.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Avg Disk"
                    value={`${trend.diskAverage.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Peak Disk"
                    value={`${trend.diskMaximum.toFixed(1)}%`}
                />

                <DashboardCard
                    title="Healthy"
                    value={trend.healthyServers}
                    statusClass="healthy"
                />

                <DashboardCard
                    title="Warning"
                    value={trend.warningServers}
                    statusClass="warning"
                />

                <DashboardCard
                    title="Critical"
                    value={trend.criticalServers}
                    statusClass="critical"
                />

                <DashboardCard
                    title="Offline"
                    value={trend.offlineServers}
                    statusClass="offline"
                />

                <DashboardCard
                    title="Alerts"
                    value={trend.totalAlertsOpened}
                />

                <DashboardCard
                    title="Gateway RTT"
                    value={`${trend.gatewayResponseAverageMs.toFixed(0)} ms`}
                />
            </div>
            <h2>Servers</h2><ServerTable servers={servers} />
        </div>
    );
}

export default DashboardPage;