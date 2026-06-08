import {
    acknowledgeAlert,
    unacknowledgeAlert,
    suppressAlert,
    unsuppressAlert
}
from "../../api/alertsApi";

export function getLastSeenAge(
    timestamp: string)
{
    const minutes =
        Math.floor(
            (
                Date.now() -
                new Date(timestamp).getTime()
            ) / 60000);

    if (minutes < 1)
        return "<1m";

    if (minutes < 60)
        return `${minutes}m`;

    const hours =
        Math.floor(minutes / 60);

    return `${hours}h`;
}

export function formatEventTimestamp(
    timestamp: string)
{
    return new Date(timestamp)
        .toLocaleString();
}

export async function handleAcknowledge(
    alertId: number,
    reload: () => Promise<void>)
{
    await acknowledgeAlert(
        alertId);

    await reload();
}

export async function handleUnacknowledge(
    alertId: number,
    reload: () => Promise<void>)
{
    await unacknowledgeAlert(
        alertId);

    await reload();
}

export async function handleSuppress(
    alertId: number,
    reload: () => Promise<void>)
{
    await suppressAlert(
        alertId,
        0.01);

    await reload();
}

export async function handleUnsuppress(
    alertId: number,
    reload: () => Promise<void>)
{
    await unsuppressAlert(
        alertId);

    await reload();
}

export function formatMinutes(minutes: number)
{
    const hours =
        Math.floor(minutes / 60);

    const remaining =
        minutes % 60;

    return `${hours}h ${remaining}m`;
}

export function getIntervalMinutes(hours: number)
{
    if (hours <= 1)
        return 1;

    if (hours <= 6)
        return 5;

    if (hours <= 24)
        return 15;

    return 60;
}
export function formatTimestamp(timestamp: string, hours: number)
{

    const date =
        new Date(timestamp);

    if (date.getFullYear() < 2000)
        return "-";

    if (hours <= 24)
        return date.toLocaleTimeString();

    return date.toLocaleDateString();
};
export function formatDate(
    value?: string)
{
    if (!value)
        return "-";

    const date =
        new Date(value);

    if (date.getFullYear() < 2000)
        return "-";

    return date.toLocaleString();
}
export function formatDuration(
    start: string,
    end?: string)
{
    const startDate =
        new Date(start);

    if (startDate.getFullYear() < 2000)
        return "-";

    const endDate =
        end
            ? new Date(end)
            : new Date();

    const ms =
        endDate.getTime() -
        startDate.getTime();

    const minutes =
        Math.floor(ms / 60000);

    const hours =
        Math.floor(minutes / 60);

    const remainingMinutes =
        minutes % 60;

    if (hours > 0)
    {
        return `${hours}h ${remainingMinutes}m`;
    }

    return `${minutes}m`;
}