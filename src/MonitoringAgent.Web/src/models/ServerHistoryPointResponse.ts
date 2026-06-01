export interface ServerHistoryPointResponse {
    timestampUtc: string;
    cpuPercent: number;
    memoryPercent: number;
    diskPercentUsed: number;

    diskReadLatencyMs: number;
    diskWriteLatencyMs: number;

    avgDiskQueueLength: number;

    diskReadsPerSec: number;
    diskWritesPerSec: number;

    networkBytesReceivedPerSec: number;
    networkBytesSentPerSec: number;

    gatewayResponseMs: number;
    ignitionMemoryMb: number;
    ignitionCpuPercent: number;
}