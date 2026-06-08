import "./ErrorMessage.css";

type Props = {
    message: string;
};

export default function ErrorMessage({
    message
}: Props) {
    return (
        <div className="error-message">
            {message}
        </div>
    );
}