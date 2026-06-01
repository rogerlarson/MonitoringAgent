import "./DashboardCard.css";
import Sparkline from "./Sparkline.tsx"

type Props = {
    title: string;
    subtitle?: string;
    value: string | number;
    statusClass?: string;
    sparklineData?: number[];
    sparklineColor?: string;
    trend?: number;
};

export default function DashboardCard(
    {
        title,
        subtitle,
        value,
        statusClass,
        sparklineData,
        sparklineColor,
        trend
    }: Props)
{
    return (
        <div className={`dashboard-card ${statusClass ?? ""}`}>
            <div className="dashboard-card-title">
                {title}
            </div>
            <div className={`dashboard-card-value ${statusClass ?? ""}`}>
                {
                    statusClass === "healthy"
                        ? "● "
                        : statusClass === "warning"
                            ? "● "
                            : statusClass === "critical"
                                ? "● "
                                : ""
                }

                {value}
            </div>
            {
                subtitle &&
                (
                    <div
                        className="dashboard-card-subtitle"
                    >
                        {subtitle}
                    </div>
                )
            }
            {
                trend !== undefined &&
                (
                    <div
                        className="dashboard-card-trend"
                    >
                        {
                            trend > 0
                                ? `▲ +${trend.toFixed(1)}%`
                                : trend < 0
                                    ? `▼ ${Math.abs(trend).toFixed(1)}%`
                                    : "▬ Stable"
                        }
                    </div>
                )
            }
            {
                sparklineData &&
                (
                    <div
                        style={{
                            height: "40px",
                            marginTop: "10px"
                        }}
                    >
                        <Sparkline
                            data={sparklineData}
                            color={
                                sparklineColor ??
                                "#22c55e"
                            }
                        />
                    </div>
                )
            }
        </div>
    );
}