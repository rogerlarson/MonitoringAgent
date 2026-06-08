export interface GatewaySnapshotResponse
{
    snapshotUtc: string;

    reachable: boolean;

    responseMs: number;

    httpStatusCode?: number;
}