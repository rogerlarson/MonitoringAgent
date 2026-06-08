// Styles
import "../App.css";
import "./AboutPage.css";

// Navigation
import AppNav 
    from "../nav/AppNav";
import PageHeader
    from "../components/PageHeader";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

// Components
import Footer
    from "../nav/Footer";

// Constants
import {
    APP_NAME,
    APP_VERSION,
    BUILD_DATE,
    BUILD_NUMBER,
    AUTHOR
}
from "../constants/Version";

import.meta.env.VITE_API_URL

/**
 * ============================================================================
 * About Page
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Displays application metadata,
 * version information, and project
 * overview.
 * ============================================================================
 */

export default function AboutPage() {
    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("About");

    return (

        <div className="page">

            <AppNav />

            {/* -------------------------------------------------------------
            Header
            ------------------------------------------------------------- */}

            <PageHeader title="About" />

            <div className="about-card">

                <h2>
                    {APP_NAME}
                </h2>

                <table className="details-table">

                    <tbody>

                        <tr>
                            <td>Version</td>
                            <td>{APP_VERSION}</td>
                        </tr>

                        <tr>
                            <td>Build Number</td>
                            <td>{BUILD_NUMBER}</td>
                        </tr>

                        <tr>
                            <td>Build Date</td>
                            <td>{BUILD_DATE}</td>
                        </tr>

                        <tr>
                            <td>Author</td>
                            <td>{AUTHOR}</td>
                        </tr>

                        <tr>
                            <td>Status</td>
                            <td>Active Development</td>
                        </tr>

                        <tr>
                            <td>Environment</td>
                            <td>{import.meta.env.MODE}</td>
                        </tr>
                        <tr>
                            <td>Repository</td>
                            <td>Internal Project</td>
                        </tr>

                    </tbody>

                </table>

                <h2>
                    Overview
                </h2>

                <p>
                    The Ignition Monitoring Platform provides
                    centralized monitoring, alerting, diagnostics,
                    and historical visibility for Ignition gateways,
                    Windows servers, and supporting infrastructure.
                </p>

                <h2>Technology Stack</h2>

                <ul>
                    <li>React</li>
                    <li>TypeScript</li>
                    <li>Vite</li>
                    <li>ASP.NET Core</li>
                    <li>SQL Server</li>
                </ul>

                <h2>
                    Major Modules
                </h2>

                <ul>
                    <li>
                        Dashboard - Fleet-wide monitoring and health overview
                    </li>

                    <li>
                        Server Details - Historical metrics and diagnostics
                    </li>

                    <li>
                        Alerts - Alert lifecycle management
                    </li>

                    <li>
                        Alert Rules - Monitoring rule configuration
                    </li>

                    <li>
                        Engine Status - Worker and service monitoring
                    </li>
                </ul>

                <h2>Project Status</h2>

                <ul>
                    <li>Monitoring Dashboard Complete</li>
                    <li>Alert Management Complete</li>
                    <li>Historical Metrics Complete</li>
                    <li>Email Notifications Complete</li>
                    <li>Authentication Planned</li>
                </ul>

            </div>

            {/* -------------------------------------------------------------
                Footer
               ------------------------------------------------------------- */}
            <Footer />

        </div>

    );
}