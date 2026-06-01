import "./MetricChart.css";

import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    CartesianGrid,
    ReferenceLine
}
from "recharts";

type AlertMarker =
{
    id: number;
    timestamp: number;
    label: string;
};

type Props = {
    title: string;
    data: any[];
    dataKey: string;
    color?: string;
    yDomain?: [number, number];
    alertMarkers?: AlertMarker[];
};

export default function MetricChart(
    {
        title,
        data,
        dataKey,
        color = "#3b82f6",
        yDomain,
        alertMarkers
    }: Props)
{
    return (
        <div className="metric-chart">
            <h3 className="metric-chart-title">
                {title}
            </h3>

            <ResponsiveContainer
                width="100%"
                height={250}
            >
                <LineChart data={data}>
                    {
                        alertMarkers?.map(marker => (
                                
                            <ReferenceLine
                                key={marker.id}
                                x={marker.timestamp}
                                stroke={getAlertColor(marker.label)}
                                strokeDasharray="4 4"
                            />
                        ))
                    }
                    <CartesianGrid
                        stroke="#333"
                        vertical={false}
                    />
                   <XAxis
                        type="number"
                        dataKey="timestamp"
                        domain={["dataMin", "dataMax"]}
                        tick={{ fontSize: 10 }}
                        tickFormatter={value =>
                            new Date(value)
                                .toLocaleTimeString()}
                    />
                    <YAxis
                        domain={yDomain}
                    />
                    <Tooltip />

                    <Line
                        type="monotone"
                        dataKey={dataKey}
                        stroke={color}
                        dot={false}
                    />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}
function getAlertColor(
    label: string)
{
    if (label.includes("Gateway"))
        return "#ef4444";

    if (label.includes("Ignition"))
        return "#f97316";

    if (label.includes("CPU"))
        return "#eab308";

    return "#94a3b8";
}