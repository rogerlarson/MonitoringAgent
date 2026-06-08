// Helpers
import {
    formatEventTimestamp
} from "./ServerDetailsHelpers";

// Models
import type {
    EventFeedItem
} from "../../models/EventFeedItem";

// Styles
import "./RecentEventsPanel.css";

type Props = {
    recentEvents: EventFeedItem[];
};

/**
 * Recent Events Panel
 *
 * Displays a chronological feed of recent server events.
 *
 * Examples:
 * - Server became unavailable
 * - Server came back online
 * - Monitoring agent disconnected
 * - Monitoring agent reconnected
 *
 * Events are displayed in reverse chronological order
 * with a visual status indicator and timestamp.
 */
export function RecentEventsPanel({
    recentEvents
}: Props) {
    return (
        <>
            {/* -----------------------------------------------------------------
                Event Feed Header
               ----------------------------------------------------------------- */}
            <h2>Recent Events</h2>

            {/* -----------------------------------------------------------------
                Event Feed
               ----------------------------------------------------------------- */}
            <div className="event-feed">
                {recentEvents.map((event) => (
                    <div
                        key={event.id}
                        className="event-item"
                    >
                        {/* Event Status Indicator */}
                        <span className={`event-icon ${event.type}`}>
                            {getEventIcon(event.type)}
                        </span>

                        {/* Event Timestamp */}
                        <span className="event-time">
                            {formatEventTimestamp(
                                event.timestamp
                            )}
                        </span>

                        {/* Event Description */}
                        <span className="event-message">
                            {event.text}
                        </span>
                    </div>
                ))}
            </div>
        </>
    );
}

function getEventIcon(type: string) {
    switch (type) {
        case "down":
            return "🟥";

        case "up":
            return "🟩";

        default:
            return "⬜";
    }
}