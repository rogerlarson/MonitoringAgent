import { api } from "./api";

import type {
    ServerResponse
}
from "../models/ServerResponse";

import type {
    ServerDetailsResponse
}
from "../models/ServerDetailsResponse";

import type { GatewayHistoryResponse }
    from "../models/GatewayHistoryResponse";

import type { IgnitionHistoryResponse }
    from "../models/IgnitionHistoryResponse";

import type { AlertHistoryResponse }
    from "../models/AlertHistoryResponse";

import type
{
    GatewaySnapshotResponse
}
from "../models/GatewaySnapshotResponse";

import type
{
    IgnitionSnapshotResponse
}
from "../models/IgnitionSnapshotResponse";

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
    intervalMinutes: number) : Promise<GatewaySnapshotResponse[]>
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
    intervalMinutes: number) : Promise<IgnitionSnapshotResponse[]>
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

