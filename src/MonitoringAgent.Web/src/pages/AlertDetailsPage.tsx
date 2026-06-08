/**
 * ============================================================================
 * Alert Details Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Detailed investigation view for an individual alert.
 *
 * Features:
 * - Alert metadata
 * - Alert lifecycle information
 * - Acknowledge workflow
 * - Suppress workflow
 * - Manual closure
 *
 * Alert States:
 * Open
 * Acknowledged
 * Suppressed
 * Closed
 *
 * Notes:
 * This page serves as the primary workflow surface for alert handling.
 * ============================================================================
 */

// Styles
import "./AlertDetailsPage.css";

// React
import {
    useEffect,
    useState
} from "react";

import {
    Link,
    useParams
} from "react-router-dom";

// Navigation
import AppNav 
    from "../nav/AppNav";
import PageHeader
    from "../components/PageHeader";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

// APIs
import {
    getAlert,
    acknowledgeAlert,
    unacknowledgeAlert,
    suppressAlert,
    unsuppressAlert,
    closeAlert
} from "../api/alertsApi";

// Models
import type {
    AlertResponse
} from "../models/AlertResponse";

// Components
import StatusBadge
    from "../components/StatusBadge";
import Footer
    from "../nav/Footer";
import LoadingIndicator 
    from "../components/LoadingIndicator";

/**
 * Alert Details Page
 *
 * Displays complete information about a single alert,
 * including:
 *
 * - Current status
 * - Severity
 * - Trigger history
 * - Notification history
 * - User actions
 *
 * Allows alert lifecycle actions:
 * - Acknowledge
 * - Suppress
 * - Unsuppress
 * - Close
 */

type AlertAction = {
    label: string;
    action: (alertId: number) => Promise<void>;
};

const alertActions: Record<
    string,
    AlertAction[]
    > = {

        Open: [
            {
                label: "Acknowledge",
                action: acknowledgeAlert
            },
            {
                label: "Suppress",
                action: suppressAlert
            },
            {
                label: "Close",
                action: closeAlert
            }
        ],

        Acknowledged: [
            {
                label: "Unacknowledge",
                action: unacknowledgeAlert
            },
            {
                label: "Close",
                action: closeAlert
            }
        ],

        Suppressed: [
            {
                label: "Unsuppress",
                action: unsuppressAlert
            },
            {
                label: "Close",
                action: closeAlert
            }
        ]
};

export default function AlertDetailsPage() {

    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("Alert Details");

    // -------------------------------------------------------------------------
    // Route Parameters
    // -------------------------------------------------------------------------

    const { id } =
        useParams();

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [alert, setAlert] =
        useState<AlertResponse | null>(
            null
        );

    // -------------------------------------------------------------------------
    // Data Loading
    // -------------------------------------------------------------------------

    const loadAlert = async () => {

        if (!id) {
            return;
        }

        const data =
            await getAlert(
                Number(id)
            );

        setAlert(
            data
        );
    };

    // -------------------------------------------------------------------------
    // Alert Actions
    // -------------------------------------------------------------------------

    const executeAction = async (
        action: (
            alertId: number
        ) => Promise<void>
    ) => {

        if (!alert) {
            return;
        }

        await action(
            alert.alertEventId
        );

        await loadAlert();
    };

    // -------------------------------------------------------------------------
    // Initial Load
    // -------------------------------------------------------------------------

    useEffect(() => {

        loadAlert();

    }, [id]);

    // -------------------------------------------------------------------------
    // Loading State
    // -------------------------------------------------------------------------

    if (!alert) {

        return <LoadingIndicator />;
    }

    // -------------------------------------------------------------------------
    // Render
    // -------------------------------------------------------------------------

    const availableActions =
    alertActions[
        alert.status
    ] ?? [];

    return (
        <div className="page">

            <AppNav />

            {/* -------------------------------------------------------------
                Header
               ------------------------------------------------------------- */}

            <PageHeader title="Alert Details">
            </PageHeader>

            {/* -------------------------------------------------------------
                Alert Header
               ------------------------------------------------------------- */}

            <div className="alert-details-header">

                <h2>
                    {alert.ruleName}
                </h2>

                {/* ---------------------------------------------------------
                    Alert Actions
                   --------------------------------------------------------- */}

                <div className="alert-details-actions">

                    {availableActions.map(action => (

                        <button
                            key={action.label}
                            onClick={() =>
                                executeAction(
                                    action.action
                                )
                            }
                        >
                            {action.label}
                        </button>

                    ))}

                </div>

                {/* ---------------------------------------------------------
                    Alert Summary
                   --------------------------------------------------------- */}

                <div className="alert-details-summary">

                    <div>
                        Server:{" "}
                        <Link
                            to={`/servers/${alert.serverId}`}
                        >
                            {alert.serverName}
                        </Link>
                    </div>

                    <div>
                        Severity:{" "}
                        {alert.severity}
                    </div>

                    <div>
                        Status:{" "}
                        <StatusBadge
                            status={alert.status}
                        />
                    </div>

                </div>

            </div>

            {/* -------------------------------------------------------------
                Alert Details Table
               ------------------------------------------------------------- */}

            <table className="details-table">

                <tbody>

                    <tr>
                        <td>Alert Id</td>
                        <td>{alert.alertEventId}</td>
                    </tr>

                    <tr>
                        <td>Message</td>
                        <td>{alert.message}</td>
                    </tr>

                    <tr>
                        <td>Occurrences</td>
                        <td>{alert.occurrenceCount}</td>
                    </tr>

                    <tr>
                        <td>Notifications</td>
                        <td>{alert.notificationCount}</td>
                    </tr>

                    <tr>
                        <td>Opened</td>
                        <td>
                            {new Date(
                                alert.openedUtc
                            ).toLocaleString()}
                        </td>
                    </tr>

                    <tr>
                        <td>Closed</td>
                        <td>
                            {alert.closedUtc
                                ? new Date(
                                    alert.closedUtc
                                ).toLocaleString()
                                : "-"}
                        </td>
                    </tr>

                    <tr>
                        <td>Last Seen</td>
                        <td>
                            {alert.lastSeenUtc
                                ? new Date(
                                    alert.lastSeenUtc
                                ).toLocaleString()
                                : "-"}
                        </td>
                    </tr>

                    <tr>
                        <td>Acknowledged By</td>
                        <td>
                            {alert.acknowledgedBy ?? "-"}
                        </td>
                    </tr>

                    <tr>
                        <td>Suppressed By</td>
                        <td>
                            {alert.suppressedBy ?? "-"}
                        </td>
                    </tr>

                    <tr>
                        <td>Closed By</td>
                        <td>
                            {alert.closedBy ?? "-"}
                        </td>
                    </tr>

                    <tr>
                        <td>First Triggered</td>
                        <td>
                            {alert.firstTriggeredUtc
                                ? new Date(
                                    alert.firstTriggeredUtc
                                ).toLocaleString()
                                : "-"}
                        </td>
                    </tr>

                </tbody>

            </table>

            {/* -------------------------------------------------------------
            Footer
            ------------------------------------------------------------- */}
            <Footer />

        </div>
    );
}