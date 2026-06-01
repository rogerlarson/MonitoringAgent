using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Services;

public static class ServerStatusCalculator
{
    public static string Calculate(
        DateTime? lastSeenUtc,
        HostSnapshotEntity? hostSnapshot)
    {
        if (lastSeenUtc == null)
        {
            return "Unknown";
        }

        if (DateTime.UtcNow - lastSeenUtc.Value >
            TimeSpan.FromSeconds(30))
        {
            return "Offline";
        }

        if (hostSnapshot == null)
        {
            return "Unknown";
        }
        if (hostSnapshot.DiskPercentUsed >= 95)
        {
            return "Critical";
        }

        if (hostSnapshot.MemoryPercent >= 95)
        {
            return "Critical";
        }

        if (hostSnapshot.CpuPercent >= 95)
        {
            return "Critical";
        }

        if (hostSnapshot.DiskReadLatencyMs >= 250 ||
            hostSnapshot.DiskWriteLatencyMs >= 250)
        {
            return "Critical";
        }

        if (hostSnapshot.AvailableMemoryMb <= 512)
        {
            return "Critical";
        }

        if (hostSnapshot.AvgDiskQueueLength >= 50)
        {
            return "Critical";
        }

        if (hostSnapshot.DiskPercentUsed >= 90)
        {
            return "Warning";
        }

        if (hostSnapshot.MemoryPercent >= 85)
        {
            return "Warning";
        }

        if (hostSnapshot.CpuPercent >= 85)
        {
            return "Warning";
        }

        if (hostSnapshot.DiskReadLatencyMs >= 50 ||
            hostSnapshot.DiskWriteLatencyMs >= 50)
        {
            return "Warning";
        }

        if (hostSnapshot.AvgDiskQueueLength >= 10)
        {
            return "Warning";
        }

        if (hostSnapshot.AvailableMemoryMb <= 2048)
        {
            return "Warning";
        }

        return "Healthy";
    }
}