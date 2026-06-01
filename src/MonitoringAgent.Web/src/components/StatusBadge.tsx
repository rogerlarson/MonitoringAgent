import "./StatusBadge.css";

type Props = {
    status: string;
};

export default function StatusBadge(
    { status }: Props)
{
    const cssClass =
        status.toLowerCase();

    return (
        <span
            className={`status-badge ${cssClass}`}>
            {status}
        </span>
    );
}