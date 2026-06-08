import type TopProblemServerResponse from './TopProblemServerResponse'

export interface DashboardTrendResponse {
    cpuAverage: number;
    cpuMaximum: number;

    memoryAverage: number;
    memoryMaximum: number;

    diskAverage: number;
    diskMaximum: number;

    gatewayResponseAverageMs: number;

    totalAlertsOpened: number;
    criticalAlertsOpened: number;
    warningAlertsOpened: number;

    healthyServers: number;
    warningServers: number;
    criticalServers: number;
    offlineServers: number;

    openAlerts: number;
    criticalAlerts: number;

    runningWorkers: number;
    stoppedWorkers: number;

    topProblemServers:
    TopProblemServerResponse[];
}