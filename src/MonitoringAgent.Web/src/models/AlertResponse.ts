export interface AlertResponse
{
    alertEventId: number;

    alertRuleId: number;

    serverId: number;

    serverServiceId?: number;

    status: string;

    message: string;

    openedUtc: string;

    closedUtc?: string;

    lastSeenUtc?: string;

    firstTriggeredUtc?: string;

    lastNotificationUtc?: string;

    occurrenceCount: number;

    acknowledgedUtc?: string;

    acknowledgedBy?: string;

    suppressedUntilUtc?: string;

    notificationCount: number;

    suppressedBy?: string;

    suppressedUtc?: string;

    closedBy?: string;
    
    serverName: string;

    ruleName: string;

    severity: string;
}