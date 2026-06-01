import "./Sparkline.css";

import {
    LineChart,
    Line,
    ResponsiveContainer,
    YAxis
}
from "recharts";

type Props = {
    data: number[];
    color: string;
};

export default function Sparkline(
    {
        data,
        color
    }: Props)
{
    const chartData =
        data.map(x => ({
            value: x
        }));

    return (
        <div className="sparkline-container">
            <ResponsiveContainer
                className="sparkline"
                width="100%"
                height={40}
            >
                <LineChart
                    data={chartData}
                    margin={{
                        top: 2,
                        right: 2,
                        bottom: 2,
                        left: 2
                    }}
                >
                    <YAxis
                        hide
                        domain={[
                            (dataMin: number) => dataMin * 0.99,
                            (dataMax: number) => dataMax * 1.01
                        ]}
                    />

                    <Line
                        type="monotone"
                        dataKey="value"
                        stroke={color}
                        dot={false}
                        strokeWidth={2}
                    />
                </LineChart>
            </ResponsiveContainer>
        </div>
    );
}