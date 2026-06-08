import { api }
from "./api";
import type
{
    AlertResponse
}
from "../models/AlertResponse";

export async function getAlerts()
{
    const response =
        await api.get(
            "/alerts");

    return response.data;
}

export async function getAlert(
    alertId: number)
    : Promise<AlertResponse>
{
    const response =
        await api.get<AlertResponse>(
            `/alerts/${alertId}`);

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

export async function closeAlert(
    alertId: number)
{
    await api.post(
        `/alerts/${alertId}/close`,
        {
            userName: "roger"
        });
}