// Models
import type { ServerDetailsResponse } from "../../models/ServerDetailsResponse";

// Components
import StatusBadge from "../../components/StatusBadge";
import LastUpdated from "../../components/LastUpdated";

// Styles
import "./Header.css";

type Props = {
    server: ServerDetailsResponse;
    lastUpdated: Date;
};

/**
 * Server Header
 *
 * Displays high-level server information including:
 * - Server name
 * - Current status
 * - Last updated timestamp
 * - Operating system
 * - CPU core count
 * - Total memory
 */
export function Header({
    server,
    lastUpdated
}: Props) {
    return (
        <>
            {/* -----------------------------------------------------------------
                Title Row
               ----------------------------------------------------------------- */}
            <div className="server-header">
                <div className="server-header__title-group">
                    <h1 className="server-header__title">
                        {server.serverName}
                    </h1>

                    <StatusBadge status={server.status} />
                </div>

                <LastUpdated value={lastUpdated} />
            </div>

            {/* -----------------------------------------------------------------
                Server Metadata
               ----------------------------------------------------------------- */}
            <div className="server-header__metadata">
                {server.operatingSystem}
                {" • "}
                {server.processorCount} cores
                {" • "}
                {(server.totalMemoryMb / 1024).toFixed(1)} GB RAM
            </div>

            {/* -----------------------------------------------------------------
                Future Action Buttons / Controls
               ----------------------------------------------------------------- */}
            <div className="server-header__actions">
                {/* Reserved for future actions */}
            </div>
        </>
    );
}