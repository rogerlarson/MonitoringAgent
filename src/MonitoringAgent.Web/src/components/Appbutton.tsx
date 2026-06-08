import "./AppButton.css";

type Props = {
    children: React.ReactNode;
    onClick?: () => void;
    type?: "button" | "submit";
    disabled?: boolean;
    className?: string;
};

export default function AppButton({
    children,
    onClick,
    type = "button",
    disabled = false,
    className = ""
}: Props) {
    return (
        <button
            type={type}
            onClick={onClick}
            disabled={disabled}
            className={`app-button ${className}`}
        >
            {children}
        </button>
    );
}