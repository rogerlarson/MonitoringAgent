import { api }
from "./api";

import type { AlertRuleResponse }
    from "../models/AlertRuleResponse";

export async function getAlertRules()
{
    const response =
        await api.get(
            "/alertrules");

    return response.data;
}

export async function saveAlertRule(
    rule: AlertRuleResponse)
{
    if (rule.alertRuleId)
    {
        await api.put(
            `/alertrules/${rule.alertRuleId}`,
            rule);
    }
    else
    {
        await api.post(
            "/alertrules",
            rule);
    }
}


export async function deleteAlertRule(
    id: number)
{
    await api.delete(
        `/alertrules/${id}`);
}

export async function setRuleEnabled(
    id: number,
    enabled: boolean)
{
    await api.post(
        `/alertrules/${id}/${enabled
            ? "enable"
            : "disable"}`);
}

export async function createAlertRule(
    rule: AlertRuleResponse)
{
    await api.post(
        "/alertrules",
        rule);
}

export async function updateAlertRule(
    rule: AlertRuleResponse)
{
    await api.put(
        `/alertrules/${rule.alertRuleId}`,
        rule);
}