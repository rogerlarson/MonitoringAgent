export interface ServerMetricHistoryPoint {
    timestamp: string;

    cpuPercent: number;
    memoryPercent: number;
    diskPercentUsed: number;

    networkReceived: number;
    networkSent: number;

    diskReadsPerSec: number;
    diskWritesPerSec: number;

    diskReadLatencyMs: number;
    diskWriteLatencyMs: number;

    avgDiskQueueLength: number;
}