export interface AlertSummaryResponse
{
    alertEventId: number;

    serverId: number;

    serverName: string;

    ruleName: string;

    severity: string;

    status: string;

    message: string;

    openedUtc: string;

    closedUtc?: string;

    occurrenceCount: number;

    acknowledged: boolean;

    suppressed: boolean;
}