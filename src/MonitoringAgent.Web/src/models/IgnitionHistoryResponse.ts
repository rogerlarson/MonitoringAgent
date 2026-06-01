export interface IgnitionHistoryResponse {
    snapshotUtc: string;
    serviceRunning: boolean;
    processRunning: boolean;
    cpuPercent: number;
    memoryMb: number;
    threadCount: number;
    handleCount: number;
    uptimeMinutes: number;
    processId: number;
}