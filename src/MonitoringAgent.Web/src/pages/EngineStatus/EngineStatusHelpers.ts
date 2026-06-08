/**
 * Returns a compact age string
 * (e.g. 15s, 5m, 2h).
 */
export function getAge(
    timestamp?: string
): string {

    if (!timestamp) {
        return "-";
    }

    const seconds = Math.floor(
        (
            Date.now() -
            new Date(timestamp).getTime()
        ) / 1000
    );

    if (seconds < 60) {
        return `${seconds}s`;
    }

    const minutes =
        Math.floor(seconds / 60);

    if (minutes < 60) {
        return `${minutes}m`;
    }

    const hours =
        Math.floor(minutes / 60);

    return `${hours}h`;
}