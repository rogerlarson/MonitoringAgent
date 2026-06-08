import type
{
    ServerHistoryPointResponse
}
from "../../models/ServerHistoryPointResponse";

import type
{
    AlertHistoryResponse
}
from "../../models/AlertHistoryResponse";

export function buildHostChartData(
    history: ServerHistoryPointResponse[])
{
    return history.map(x => ({
        timestamp:
            new Date(
                x.timestampUtc)
                .getTime(),

        cpuPercent:
            x.cpuPercent,

        memoryPercent:
            x.memoryPercent,

        diskPercentUsed:
            x.diskPercentUsed,

        diskReadsPerSec:
            x.diskReadsPerSec,

        diskWritesPerSec:
            x.diskWritesPerSec,

        avgDiskQueueLength:
            x.avgDiskQueueLength,

        diskReadLatencyMs:
            x.diskReadLatencyMs,

        diskWriteLatencyMs:
            x.diskWriteLatencyMs,

        networkReceived:
            x.networkBytesReceivedPerSec /
            1024 /
            1024,

        networkSent:
            x.networkBytesSentPerSec /
            1024 /
            1024,

        gatewayResponseMs:
            x.gatewayResponseMs
    }));
}

export function buildGatewayChartData(
    gatewayHistory: any[])
{
    return gatewayHistory.map(x => ({
        timestamp:
            new Date(
                x.snapshotUtc)
                .getTime(),

        responseMs:
            x.responseMs
    }));
}

export function buildIgnitionChartData(
    ignitionHistory: any[])
{
    return ignitionHistory.map(x => ({
        timestamp:
            new Date(
                x.snapshotUtc)
                .getTime(),

        memoryMb:
            x.memoryMb,

        cpuPercent:
            x.cpuPercent,

        threadCount:
            x.threadCount,

        handleCount:
            x.handleCount
    }));
}

export function buildAlertMarkers(
    recentAlerts: AlertHistoryResponse[])
{
    return recentAlerts.map(alert => ({
        id:
            alert.alertEventId,

        timestamp:
            new Date(
                alert.firstTriggeredUtc ??
                alert.openedUtc)
                .getTime(),

        label:
            alert.ruleName
    }));
}