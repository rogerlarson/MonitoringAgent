import { api } from "./api";

import type {
    ServerResponse
}
from "../models/ServerResponse";

import type {
    ServerDetailsResponse
}
from "../models/ServerDetailsResponse";

import type {
    ServerHistoryPointResponse
}
from "../models/ServerHistoryPointResponse";

import type { GatewayHistoryResponse }
    from "../models/GatewayHistoryResponse";

import type { IgnitionHistoryResponse }
    from "../models/IgnitionHistoryResponse";

import type { AlertHistoryResponse }
    from "../models/AlertHistoryResponse";

export async function getServers()
{
    const response =
        await api.get<ServerResponse[]>(
            "/servers");

    return response.data;
}

export async function getServer(
    id: number)
{
    const response =
        await api.get<ServerDetailsResponse>(
            `/servers/${id}`);

    return response.data;
}

export async function getHostHistory(
    id: number,
    hours: number,
    intervalMinutes: number)
{
    const response =
        await api.get(
            `/servers/${id}/history?hours=${hours}&intervalMinutes=${intervalMinutes}`);

    return response.data;
}

export async function getGatewayHistory(
    id: number,
    hours: number,
    intervalMinutes: number)
{
    const response =
        await api.get<
            GatewayHistoryResponse[]
        >(`/servers/${id}/gateway-history?hours=${hours}&intervalMinutes=${intervalMinutes}`);

    return response.data;
}

export async function getIgnitionHistory(
    id: number,
    hours: number,
    intervalMinutes: number)
{
    const response =
        await api.get<
            IgnitionHistoryResponse[]
        >(`/servers/${id}/ignition-history?hours=${hours}&intervalMinutes=${intervalMinutes}`);

    return response.data;
}

export async function getAlerts(
    id: number)
{
    const response =
        await api.get<
            AlertHistoryResponse[]
        >(
            `/servers/${id}/alerts`);

    return response.data;
}

export async function acknowledgeAlert(
    alertId: number)
{
    await api.post(
        `/alerts/${alertId}/acknowledge`,
        {});
}

export async function unacknowledgeAlert(
    alertId: number)
{
    await api.post(
        `/alerts/${alertId}/unacknowledge`);
}

export async function suppressAlert(
    alertId: number,
    hours: number = 4)
{
    await api.post(
        `/alerts/${alertId}/suppress`,
        {
            hours
        });
}

export async function unsuppressAlert(
    alertId: number)
{
    await api.post(
        `/alerts/${alertId}/unsuppress`);
}