/**
 * ============================================================================
 * Engine Status Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Monitor background processing services and workers.
 *
 * Features:
 * - Worker health
 * - Run counts
 * - Error counts
 * - Last successful execution
 *
 * Notes:
 * Intended for diagnosing monitoring engine and scheduler issues.
 * ============================================================================
 */

// Styles
import "./EngineStatusPage.css";

// React
import {
    useEffect,
    useState
} from "react";

// Navigation
import AppNav 
    from "../nav/AppNav";
import PageHeader
    from "../components/PageHeader";
import Footer
    from "../nav/Footer";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

// Components
import DashboardCard from "../components/DashboardCard";
import LastUpdated from "../components/LastUpdated";

// APIs
import {
    getEngineStatus
} from "../api/engineApi";

// Models
import type {
    EngineServiceResponse
} from "../models/EngineServiceResponse";

// Helpers
import {
    getAge
} from "./EngineStatus/EngineStatusHelpers";

/**
 * Engine Status Page
 *
 * Displays health and execution statistics
 * for all monitored engine services.
 *
 * Includes:
 * - Running / Stopped counts
 * - Total executions
 * - Error totals
 * - Per-service status details
 */
export default function EngineStatusPage() {

    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("Engine Status");

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [services, setServices] =
        useState<EngineServiceResponse[]>([]);

    const [lastUpdated, setLastUpdated] =
        useState(new Date());

    // -------------------------------------------------------------------------
    // Data Loading
    // -------------------------------------------------------------------------

    const loadData = () => {

        getEngineStatus()
            .then(data => {

                setServices(data);

                setLastUpdated(
                    new Date()
                );
            });
    };

    // -------------------------------------------------------------------------
    // Auto Refresh
    // -------------------------------------------------------------------------

    useEffect(() => {

        loadData();

        const timer =
            setInterval(
                loadData,
                30000
            );

        return () =>
            clearInterval(timer);

    }, []);

    // -------------------------------------------------------------------------
    // Summary Metrics
    // -------------------------------------------------------------------------

    const running =
        services.filter(
            service =>
                service.status === "Running"
        );

    const stopped =
        services.filter(
            service =>
                service.status !== "Running"
        );

    const totalRuns =
        services.reduce(
            (sum, service) =>
                sum + service.runCount,
            0
        );

    const totalErrors =
        services.reduce(
            (sum, service) =>
                sum + service.errorCount,
            0
        );

    // -------------------------------------------------------------------------
    // Render
    // -------------------------------------------------------------------------

    return (
        <div className="page">

            <AppNav />

            {/* -------------------------------------------------------------
                Header
               ------------------------------------------------------------- */}

            <PageHeader title="Engine Status">
                <LastUpdated value={lastUpdated} />
            </PageHeader>

            {/* -------------------------------------------------------------
                Summary Cards
               ------------------------------------------------------------- */}

            <div className="engine-status-summary">

                <DashboardCard
                    title="Running"
                    value={running.length}
                    statusClass="healthy"
                />

                <DashboardCard
                    title="Stopped"
                    value={stopped.length}
                    statusClass="critical"
                />

                <DashboardCard
                    title="Total Runs"
                    value={totalRuns}
                />

                <DashboardCard
                    title="Errors"
                    value={totalErrors}
                    statusClass={
                        totalErrors > 0
                            ? "critical"
                            : "neutral"
                    }
                />

            </div>

            {/* -------------------------------------------------------------
                Service Details
               ------------------------------------------------------------- */}

            <table className="details-table">

                <thead>
                    <tr>
                        <th>Service</th>
                        <th>Status</th>
                        <th>Runs</th>
                        <th>Errors</th>
                        <th>Last Success</th>
                        <th>Age</th>
                    </tr>
                </thead>

                <tbody>

                    {services.map(service => (

                        <tr
                            key={service.serviceName}
                        >
                            <td>
                                {service.serviceName}
                            </td>

                            <td>
                                {service.status}
                            </td>

                            <td>
                                {service.runCount}
                            </td>

                            <td>
                                {service.errorCount}
                            </td>

                            <td>
                                {service.lastSuccessUtc ?? "-"}
                            </td>

                            <td>
                                {getAge(
                                    service.lastSuccessUtc
                                )}
                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

            {/* -------------------------------------------------------------
            Footer
            ------------------------------------------------------------- */}
            <Footer />

        </div>
    );
}