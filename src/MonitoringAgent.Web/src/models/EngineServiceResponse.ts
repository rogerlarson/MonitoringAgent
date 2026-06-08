// models/EngineServiceResponse.ts

export interface EngineServiceResponse
{
    serviceName: string;

    status: string;

    runCount: number;

    errorCount: number;

    lastSuccessUtc?: string;
}