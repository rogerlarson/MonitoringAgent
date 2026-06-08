/**
 * ============================================================================
 * Ignition Monitoring Platform
 * ============================================================================
 *
 * Author: Roger Larson
 * Date: 06/07/2026
 *
 * Purpose:
 * Root application component and routing entry point.
 *
 * Application Areas:
 * - Dashboard
 * - Server Details
 * - Alerts
 * - Alert Rules
 * - Engine Status
 *
 * Architecture:
 * React + TypeScript frontend
 * ASP.NET Core backend
 * SQL Server persistence
 *
 * Notes:
 * Route definitions are maintained in routes/AppRoutes.tsx.
 * ============================================================================
 */

import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";

import {
    AppRoutes
} from "./routes/AppRoutes";

// Components
import ErrorBoundary from "./components/ErrorBoundary";

export default function App() {

    return (
        <ErrorBoundary>
            <BrowserRouter>

                <Routes>

                    {AppRoutes.map(route => (

                        <Route
                            key={route.path}
                            path={route.path}
                            element={route.element}
                        />

                    ))}

                </Routes>

            </BrowserRouter>
        </ErrorBoundary>
    );
}