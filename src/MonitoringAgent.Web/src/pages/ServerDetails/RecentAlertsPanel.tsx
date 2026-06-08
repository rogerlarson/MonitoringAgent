// Helpers
import {
    formatDuration,
    formatDate
} from "./ServerDetailsHelpers";

// Models
import type {
    AlertHistoryResponse
} from "../../models/AlertHistoryResponse";

// Components
import AlertStatusBadge from "../../components/AlertStatusBadge";

// Styles
import "./RecentAlertsPanel.css";

type Props = {
    recentAlerts: AlertHistoryResponse[];
};

/**
 * Recent Alerts Panel
 *
 * Displays recently closed or historical alerts
 * associated with the current server.
 *
 * Information includes:
 * - Current alert status
 * - Alert rule name
 * - Number of occurrences
 * - Triggered, opened, and closed timestamps
 * - Total alert duration
 */
export function RecentAlertsPanel({
    recentAlerts
}: Props) {
    return (
        <>
            {/* -----------------------------------------------------------------
                Header
               ----------------------------------------------------------------- */}
            <h2>Recent Alerts</h2>

            {/* -----------------------------------------------------------------
                Empty State
               ----------------------------------------------------------------- */}
            {recentAlerts.length === 0 ? (
                <div className="empty-state">
                    No recent alerts.
                </div>
            ) : (
                <>
                    {/* ---------------------------------------------------------
                        Alert History Table
                       --------------------------------------------------------- */}
                    <table className="alert-table">
                        <thead>
                            <tr>
                                <th className="status-column">
                                    Status
                                </th>

                                <th className="rule-column">
                                    Rule
                                </th>

                                <th className="occurrences-column">
                                    Occurrences
                                </th>

                                <th className="date-column">
                                    Triggered
                                </th>

                                <th className="date-column">
                                    Opened
                                </th>

                                <th className="date-column">
                                    Closed
                                </th>

                                <th>
                                    Duration
                                </th>
                            </tr>
                        </thead>

                        <tbody>
                            {recentAlerts.map((alert) => (
                                <tr
                                    key={alert.alertEventId}
                                    title={alert.message}
                                >
                                    <td>
                                        <AlertStatusBadge
                                            status={alert.status}
                                        />
                                    </td>

                                    <td>
                                        {alert.ruleName}
                                    </td>

                                    <td>
                                        <span className="occurrence-badge">
                                            {alert.occurrenceCount}
                                        </span>
                                    </td>

                                    <td>
                                        {formatDate(
                                            alert.firstTriggeredUtc
                                        )}
                                    </td>

                                    <td>
                                        {formatDate(
                                            alert.openedUtc
                                        )}
                                    </td>

                                    <td>
                                        {formatDate(
                                            alert.closedUtc
                                        )}
                                    </td>

                                    <td>
                                        {formatDuration(
                                            alert.firstTriggeredUtc ??
                                                alert.openedUtc,
                                            alert.closedUtc
                                        )}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </>
            )}
        </>
    );
}