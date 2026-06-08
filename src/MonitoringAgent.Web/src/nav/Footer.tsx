// Styles
import "./Footer.css";

// Constants
import {
    APP_NAME,
    APP_VERSION,
    BUILD_NUMBER
}
from "../constants/Version";

/**
 * Application Footer
 *
 * Displays version and ownership
 * information across all pages.
 */

export default function Footer() {

    return (

        <footer className="app-footer">

            <div>
                {APP_NAME}
            </div>

            <div>
                v{APP_VERSION}
                {" "}
                (Build {BUILD_NUMBER})
            </div>

            <div>
                © 2026
            </div>

        </footer>

    );
}