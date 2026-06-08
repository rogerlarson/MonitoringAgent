// Helpers
import {
    handleAcknowledge,
    handleUnacknowledge,
    handleSuppress,
    handleUnsuppress,
    formatDuration
} from "./ServerDetailsHelpers";

// Models
import type { AlertHistoryResponse } from "../../models/AlertHistoryResponse";

// Components
import AlertStatusBadge from "../../components/AlertStatusBadge";

// Styles
import "./OpenAlertsPanel.css";

type Props = {
    openAlerts: AlertHistoryResponse[];
    loadData: () => Promise<void>;
};

/**
 * Open Alerts Panel
 *
 * Displays all currently open alerts for the server and
 * provides alert lifecycle actions:
 *
 * - Acknowledge
 * - Unacknowledge
 * - Suppress
 * - Unsuppress
 */
export function OpenAlertsPanel({
    openAlerts,
    loadData
}: Props) {
    return (
        <>
            {/* -----------------------------------------------------------------
                Open Alerts
               ----------------------------------------------------------------- */}
            <h2>Open Alerts</h2>

            <div className="open-alerts">
                {openAlerts.map((alert) => {
                    const cardClass =
                        alert.status === "Acknowledged"
                            ? "open-alert-card acknowledged"
                            : alert.status === "Suppressed"
                                ? "open-alert-card suppressed"
                                : "open-alert-card open";

                    return (
                        <div
                            key={alert.alertEventId}
                            className={cardClass}
                            title={alert.message}
                        >
                            {/* Alert Status */}
                            <AlertStatusBadge
                                status={alert.status}
                            />

                            {/* Alert Details */}
                            <div className="open-alert-content">
                                <strong>
                                    {alert.ruleName}
                                </strong>

                                <div>
                                    Open for{" "}
                                    {formatDuration(
                                        alert.firstTriggeredUtc ??
                                        alert.openedUtc,
                                        alert.closedUtc
                                    )}
                                </div>

                                <div className="open-alert-occurrences">
                                    {alert.occurrenceCount} occurrences
                                </div>
                            </div>

                            {/* Alert Actions */}
                            <div className="open-alert-actions">
                                {alert.status === "Open" && (
                                    <>
                                        <button
                                            onClick={() =>
                                                handleAcknowledge(
                                                    alert.alertEventId,
                                                    loadData
                                                )
                                            }
                                        >
                                            Acknowledge
                                        </button>

                                        <button
                                            onClick={() =>
                                                handleSuppress(
                                                    alert.alertEventId,
                                                    loadData
                                                )
                                            }
                                        >
                                            Suppress
                                        </button>
                                    </>
                                )}

                                {alert.status === "Acknowledged" && (
                                    <>
                                        <button
                                            onClick={() =>
                                                handleUnacknowledge(
                                                    alert.alertEventId,
                                                    loadData
                                                )
                                            }
                                        >
                                            Unacknowledge
                                        </button>

                                        <button
                                            onClick={() =>
                                                handleSuppress(
                                                    alert.alertEventId,
                                                    loadData
                                                )
                                            }
                                        >
                                            Suppress
                                        </button>
                                    </>
                                )}

                                {alert.status === "Suppressed" && (
                                    <button
                                        onClick={() =>
                                            handleUnsuppress(
                                                alert.alertEventId,
                                                loadData
                                            )
                                        }
                                    >
                                        Unsuppress
                                    </button>
                                )}
                            </div>
                        </div>
                    );
                })}
            </div>
        </>
    );
}