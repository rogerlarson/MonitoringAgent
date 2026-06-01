export interface GatewayHistoryResponse {
    snapshotUtc: string;
    reachable: boolean;
    httpStatusCode: number;
    responseMs: number;
}