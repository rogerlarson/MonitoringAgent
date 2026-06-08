// Styles
import "./LastUpdated.css";

type Props = {
    value: Date;
};

/**
 * Last Updated
 *
 * Displays the timestamp of the most recent
 * successful page refresh.
 */
export default function LastUpdated({
    value
}: Props) {

    return (
        <div className="last-updated">

            Last Updated:{" "}

            {value.toLocaleTimeString()}

        </div>
    );
}