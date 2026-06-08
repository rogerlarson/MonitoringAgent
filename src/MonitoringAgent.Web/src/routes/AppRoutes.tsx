// Routing
import {
    Route
} from "react-router-dom";

// Pages
import DashboardPage
    from "../pages/DashboardPage";

import ServerDetailsPage
    from "../pages/ServerDetailsPage";

import AlertRulesPage
    from "../pages/AlertRulesPage";

import AlertsPage
    from "../pages/AlertsPage";

import EngineStatusPage
    from "../pages/EngineStatusPage";

import AlertDetailsPage
    from "../pages/AlertDetailsPage";

import AboutPage
    from "../pages/AboutPage";    

import NotFoundPage
    from "../pages/NotFoundPage";

/**
 * Application Route Definitions
 */
export const AppRoutes = [
    {
        path: "/",
        element: <DashboardPage />
    },
    {
        path: "/servers/:id",
        element: <ServerDetailsPage />
    },
    {
        path: "/rules",
        element: <AlertRulesPage />
    },
    {
        path: "/alerts",
        element: <AlertsPage />
    },
    {
        path: "/alerts/:id",
        element: <AlertDetailsPage />
    },
    {
        path: "/engine-status",
        element: <EngineStatusPage />
    },
    {
        path: "/about",
        element: <AboutPage />
    },
    {
        path: "*",
        element: <NotFoundPage />
    }
];

/**
 * Route Elements
 */
export const RouteElements =
    AppRoutes.map(route => (
        <Route
            key={route.path}
            path={route.path}
            element={route.element}
        />
    ));