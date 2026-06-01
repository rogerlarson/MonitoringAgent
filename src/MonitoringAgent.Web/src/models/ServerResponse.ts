export interface ServerResponse {
    serverId: number;
    serverName: string;
    lastSeenUtc: string;
    status: string;
    cpuPercent: number;
    memoryPercent: number;
    diskPercentUsed: number;
    gatewayReachable: boolean;
}