export interface ServerDetailsResponse {
    serverId: number;
    serverName: string;

    lastSeenUtc: string;

    status: string;

    serviceCount: number;

    domainName: string;

    agentVersion: string;

    operatingSystem: string;

    operatingSystemVersion: string;

    processorCount: number;

    totalMemoryMb: number;

    host: {
        cpuPercent: number;
        memoryPercent: number;
        diskPercentUsed: number;
        availableMemoryMb: number;
        systemUptimeMinutes: number;
    };

    gateway: {
        reachable: boolean;
        httpStatusCode: number;
        responseMs: number;
    };

    ignition: {
        processRunning: boolean;
        serviceRunning: boolean;
        ignitionVersion: string;
        javaVersion: string;
        cpuPercent: number;
        memoryMb: number;
        uptimeMinutes: number;
    };

    openAlerts: any[];
}