// Styles
import "./AppNav.css";

// Routing
import {
    Link,
    useLocation
} from "react-router-dom";

/**
 * Application Navigation
 *
 * Shared navigation displayed at the top
 * of all primary application pages.
 */

const navItems = [
    {
        path: "/",
        label: "Dashboard"
    },
    {
        path: "/alerts",
        label: "Alerts"
    },
    {
        path: "/rules",
        label: "Alert Rules"
    },
    {
        path: "/engine-status",
        label: "Engine Status"
    },
    {
        path: "/about",
        label: "About"
    }
];

export default function AppNav() {

    const location =
        useLocation();

    const isActive =
        (path: string) =>
            location.pathname === path;

    return (

        <nav className="app-nav">

            {navItems.map(item => (

                <Link
                    key={item.path}
                    to={item.path}
                >

                    <button
                        className={
                            isActive(item.path)
                                ? "range-button active"
                                : "range-button"
                        }
                    >
                        {item.label}
                    </button>

                </Link>

            ))}

        </nav>

    );
}