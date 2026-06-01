using System.Management;

namespace MonitoringAgent.Agent.Infrastructure;

/// <summary>
/// Helper methods for Windows services.
/// </summary>
public static class WindowsServiceHelper
{
    /// <summary>
    /// Returns the process identifier for a Windows service.
    /// </summary>
    public static int? GetServiceProcessId(
        string serviceName)
    {
        using var searcher =
            new ManagementObjectSearcher(
                $"SELECT ProcessId FROM Win32_Service WHERE Name='{serviceName}'");

        foreach (ManagementObject service in searcher.Get())
        {
            return Convert.ToInt32(
                service["ProcessId"]);
        }

        return null;
    }
}