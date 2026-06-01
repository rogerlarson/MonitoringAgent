export type AlertHistoryResponse = {
    alertEventId: number;
    status: string;
    ruleName: string;
    message: string;
    openedUtc: string;
    closedUtc?: string;
    occurrenceCount: number;
    firstTriggeredUtc?: string;
};