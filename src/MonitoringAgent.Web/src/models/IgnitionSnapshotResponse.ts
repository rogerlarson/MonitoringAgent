export interface IgnitionSnapshotResponse
{
    snapshotUtc: string;

    serviceRunning: boolean;

    processRunning: boolean;

    memoryMb: number;

    cpuPercent: number;

    threadCount: number;

    handleCount: number;

    ignitionVersion?: string;
}