import "./AlertStatusBadge.css";

type Props = {
    status: string;
};

export default function AlertStatusBadge(
    {
        status
    }: Props)
{
    return (
        <span
            className={
                `alert-status-badge ${status.toLowerCase()}`
            }
        >
            {status}
        </span>
    );
}