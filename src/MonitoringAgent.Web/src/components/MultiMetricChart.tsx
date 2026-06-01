import "./MultiMetricChart.css";
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    Legend,
    ResponsiveContainer,
    CartesianGrid
}
from "recharts";

type ChartLine = {
    key: string;
    color: string;
    name: string;
};

type Props = {
    title: string;
    data: any[];
    lines: ChartLine[];
};

export default function MultiMetricChart(
    {
        title,
        data,
        lines
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
                    <CartesianGrid
                        stroke="#333"
                        vertical={false}
                    />
                    <XAxis
                        dataKey="time"
                        tick={{ fontSize: 10 }}
                        minTickGap={75}
                    />

                    <YAxis />

                    <Tooltip />

                    <Legend />

                    {
                        lines.map(line => (
                            <Line
                                key={line.key}
                                type="monotone"
                                dataKey={line.key}
                                stroke={line.color}
                                dot={false}
                                name={line.name}
                            />
                        ))
                    }

                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}