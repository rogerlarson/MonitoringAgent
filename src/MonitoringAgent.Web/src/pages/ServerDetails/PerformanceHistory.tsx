import type {
    Dispatch,
    SetStateAction
} from "react";

// Components
import MetricChart from "../../components/MetricChart";
import MultiMetricChart from "../../components/MultiMetricChart";
import ChartCard from "../../components/ChartCard";

// Styles
import "./PerformanceHistory.css";

type Props = {
    hours: number;
    setHours: Dispatch<SetStateAction<number>>;
    chartData: any[];
    gatewayChartData: any[];
    ignitionChartData: any[];
    alertMarkers: any[];
};

const ranges = [
    { hours: 1, label: "1 Hour" },
    { hours: 6, label: "6 Hours" },
    { hours: 24, label: "24 Hours" },
    { hours: 168, label: "7 Days" }
];

/**
 * Performance History
 *
 * Displays historical performance metrics for:
 *
 * - Host resources
 * - Network activity
 * - Storage activity
 * - Ignition metrics
 * - Gateway response metrics
 *
 * Users can switch between several time ranges
 * to view historical trends.
 */
export function PerformanceHistory({
    hours,
    setHours,
    chartData,
    gatewayChartData,
    ignitionChartData,
    alertMarkers
}: Props) {
    return (
        <>
            {/* -----------------------------------------------------------------
                Header
               ----------------------------------------------------------------- */}
            <h2>Performance History</h2>

            {/* -----------------------------------------------------------------
                Time Range Selector
               ----------------------------------------------------------------- */}
            <div className="performance-history__range-selector">
                {ranges.map((range) => (
                    <button
                        key={range.hours}
                        className={
                            hours === range.hours
                                ? "range-button active"
                                : "range-button"
                        }
                        onClick={() => setHours(range.hours)}
                    >
                        {range.label}
                    </button>
                ))}
            </div>

            {/* -----------------------------------------------------------------
                Host Metrics
               ----------------------------------------------------------------- */}
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

            {/* -----------------------------------------------------------------
                Network Metrics
               ----------------------------------------------------------------- */}
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

            {/* -----------------------------------------------------------------
                Storage Metrics
               ----------------------------------------------------------------- */}
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

            {/* -----------------------------------------------------------------
                Ignition Metrics
               ----------------------------------------------------------------- */}
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

            {/* -----------------------------------------------------------------
                Gateway Metrics
               ----------------------------------------------------------------- */}
            <ChartCard title="Gateway">
                <MetricChart
                    title="Gateway Response Time"
                    data={gatewayChartData}
                    dataKey="responseMs"
                    color="#a855f7"
                />
            </ChartCard>
        </>
    );
}