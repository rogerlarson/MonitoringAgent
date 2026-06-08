// Utilities
import { formatMinutes } from "./ServerDetailsHelpers";

// Models
import type { ServerDetailsResponse } from "../../models/ServerDetailsResponse";

// Components
import StatusBadge from "../../components/StatusBadge";

// Styles
import "./InfrastructurePanel.css";

type Props = {
    server: ServerDetailsResponse;
};

/**
 * Infrastructure Panel
 *
 * Displays infrastructure-level information about the server,
 * including:
 *
 * - System information
 * - Host performance metrics
 * - Ignition service details
 * - Gateway connectivity details
 */
export function InfrastructurePanel({ server }: Props) {
    return (
        <div className="infrastructure-panel">
            {/* -----------------------------------------------------------------
                System Information
               ----------------------------------------------------------------- */}
            <h2 className="panel-section-title">
                System Information
            </h2>

            <table className="details-table">
                <tbody>
                    <tr>
                        <td>Status</td>
                        <td>
                            <StatusBadge status={server.status} />
                        </td>
                    </tr>

                    <tr>
                        <td>Domain</td>
                        <td>{server.domainName}</td>
                    </tr>

                    <tr>
                        <td>Agent Version</td>
                        <td>{server.agentVersion}</td>
                    </tr>

                    <tr>
                        <td>Operating System</td>
                        <td>{server.operatingSystem}</td>
                    </tr>

                    <tr>
                        <td>OS Version</td>
                        <td>{server.operatingSystemVersion}</td>
                    </tr>

                    <tr>
                        <td>CPU Cores</td>
                        <td>{server.processorCount}</td>
                    </tr>

                    <tr>
                        <td>Total Memory</td>
                        <td>{server.totalMemoryMb?.toLocaleString()} MB</td>
                    </tr>

                    <tr>
                        <td>Last Seen</td>
                        <td>
                            {new Date(server.lastSeenUtc).toLocaleString()}
                        </td>
                    </tr>
                </tbody>
            </table>

            {/* -----------------------------------------------------------------
                Host Metrics
               ----------------------------------------------------------------- */}
            <h2 className="panel-section-title">
                Host Metrics
            </h2>

            <table className="details-table">
                <tbody>
                    <tr>
                        <td>CPU Usage</td>
                        <td>{server.host.cpuPercent}%</td>
                    </tr>

                    <tr>
                        <td>Memory Usage</td>
                        <td>{server.host.memoryPercent}%</td>
                    </tr>

                    <tr>
                        <td>Disk Usage</td>
                        <td>{server.host.diskPercentUsed}%</td>
                    </tr>

                    <tr>
                        <td>Available Memory</td>
                        <td>
                            {server.host.availableMemoryMb?.toLocaleString()} MB
                        </td>
                    </tr>

                    <tr>
                        <td>System Uptime</td>
                        <td>
                            {formatMinutes(
                                server.host.systemUptimeMinutes
                            )}
                        </td>
                    </tr>
                </tbody>
            </table>

            {/* -----------------------------------------------------------------
                Ignition Service
               ----------------------------------------------------------------- */}
            <h2 className="panel-section-title">
                Ignition
            </h2>

            <table className="details-table">
                <tbody>
                    <tr>
                        <td>Process Running</td>
                        <td>
                            {server.ignition?.processRunning
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>Service Running</td>
                        <td>
                            {server.ignition?.serviceRunning
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>Version</td>
                        <td>
                            {server.ignition?.ignitionVersion}
                        </td>
                    </tr>

                    <tr>
                        <td>Memory</td>
                        <td>
                            {server.ignition?.memoryMb} MB
                        </td>
                    </tr>
                </tbody>
            </table>

            {/* -----------------------------------------------------------------
                Gateway Connectivity
               ----------------------------------------------------------------- */}
            <h2 className="panel-section-title">
                Gateway
            </h2>

            <table className="details-table">
                <tbody>
                    <tr>
                        <td>Reachable</td>
                        <td>
                            {server.gateway?.reachable
                                ? "Yes"
                                : "No"}
                        </td>
                    </tr>

                    <tr>
                        <td>HTTP Status</td>
                        <td>
                            {server.gateway?.httpStatusCode}
                        </td>
                    </tr>

                    <tr>
                        <td>Response Time</td>
                        <td>
                            {server.gateway?.responseMs} ms
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
}