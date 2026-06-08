import type
{
    ServerDetailsResponse
}
from "../../models/ServerDetailsResponse";

export function getGatewayStatus(
    server: ServerDetailsResponse)
{
    return !server.gateway?.reachable
        ? "critical"
        : (server.gateway.responseMs ?? 0) > 250
            ? "warning"
            : "healthy";
}

export function getGatewayText(
    server: ServerDetailsResponse)
{
    return !server.gateway?.reachable
        ? "Offline"
        : (server.gateway.responseMs ?? 0) > 500
            ? "Slow"
            : "Healthy";
}

export function getIgnitionStatus(
    server: ServerDetailsResponse)
{
    return !server.ignition?.serviceRunning
        ? "critical"
        : (server.ignition.memoryMb ?? 0) > 4096
            ? "warning"
            : "healthy";
}

export function getIgnitionText(
    server: ServerDetailsResponse)
{
    return !server.ignition?.serviceRunning
        ? "Stopped"
        : (server.ignition.memoryMb ?? 0) > 4096
            ? "High Memory"
            : "Running";
}

export function getAgentStatus(
    server: ServerDetailsResponse)
{
    const minutesSinceLastSeen =
        Math.floor(
            (
                Date.now() -
                new Date(
                    server.lastSeenUtc)
                    .getTime()
            ) / 60000);

    return minutesSinceLastSeen > 5
        ? "critical"
        : minutesSinceLastSeen > 2
            ? "warning"
            : "healthy";
}

export function getAgentText(
    server: ServerDetailsResponse)
{
    const minutesSinceLastSeen =
        Math.floor(
            (
                Date.now() -
                new Date(
                    server.lastSeenUtc)
                    .getTime()
            ) / 60000);

    return minutesSinceLastSeen > 5
        ? "Offline"
        : minutesSinceLastSeen > 2
            ? "Delayed"
            : "Online";
}