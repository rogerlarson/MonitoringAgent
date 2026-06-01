// components/ChartCard.tsx

type Props = {
    title: string;
    children: React.ReactNode;
};

export default function ChartCard(
    {
        title,
        children
    }: Props)
{
    return (
        <div className="chart-card">

            <h4 className="chart-card-title">
                {title}
            </h4>

            {children}

        </div>
    );
}