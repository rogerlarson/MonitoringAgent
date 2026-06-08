// api/engineApi.ts

import { api }
    from "./api";

export async function getEngineStatus()
{
    const response =
        await api.get(
            "/engine/status");

    return response.data;
}