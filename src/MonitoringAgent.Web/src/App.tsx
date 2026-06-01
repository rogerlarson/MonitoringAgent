import {
    BrowserRouter,
    Routes,
    Route
}
from "react-router-dom";

import DashboardPage
    from "./pages/DashboardPage";

import ServerDetailsPage
    from "./pages/ServerDetailsPage";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>

                <Route
                    path="/"
                    element={<DashboardPage />}
                />

                <Route
                    path="/servers/:id"
                    element={<ServerDetailsPage />}
                />

            </Routes>
        </BrowserRouter>
    );
}