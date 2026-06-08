// Navigation
import AppNav from "../nav/AppNav";
import {
    usePageTitle
}
from "../hooks/UsePageTitle";

// Components
import PageHeader from "../components/PageHeader";
import Footer from "../nav/Footer";

export default function NotFoundPage() {
    // -------------------------------------------------------------------------
    // Page Title
    // -------------------------------------------------------------------------
    usePageTitle("404 - Page Not Found");

    return (
        <div className="page">

            <AppNav />

            <PageHeader
                title="404 - Page Not Found"
            />

            <div
                className="about-card"
            >
                <p>
                    The page you requested
                    does not exist.
                </p>

                <p>
                    Please use the navigation
                    above to continue.
                </p>
            </div>

            <Footer />

        </div>
    );
}