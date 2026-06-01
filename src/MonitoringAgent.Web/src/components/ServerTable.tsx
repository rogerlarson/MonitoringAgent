import "./ServerTable.css";
import type { ServerResponse } from "../models/ServerResponse";
import StatusBadge from "./StatusBadge";
import { Link } from "react-router-dom";

type Props = {
    servers: ServerResponse[];
};

export default function ServerTable(
    { servers }: Props)
{
    return (
        <table className="server-table">
            <thead>
                <tr>
                    <th>Server</th>
                    <th>Status</th>
                    <th>CPU</th>
                    <th>Memory</th>
                    <th>Disk</th>
                    <th>Gateway</th>
                    <th>Last Seen</th>
                </tr>
            </thead>

            <tbody>
                {servers.map(server => (
                    <tr key={server.serverId}>
                        <td>
                            <Link
                                to={`/servers/${server.serverId}`}
                                style={{
                                    color: "#60a5fa",
                                    textDecoration: "none"
                                }}
                            >
                                {server.serverName}
                            </Link>
                        </td>

                        <td>
                            <StatusBadge
                                status={server.status}
                            />
                        </td>

                        <td>
                            {server.cpuPercent.toFixed(1)}%
                        </td>

                        <td>
                            {server.memoryPercent.toFixed(1)}%
                        </td>

                        <td>
                            {server.diskPercentUsed.toFixed(1)}%
                        </td>

                        <td>
                            {server.gatewayReachable
                                ? "Online"
                                : "Offline"}
                        </td>

                        <td>
                            {new Date(
                                server.lastSeenUtc)
                                .toLocaleString()}
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}