// ============================================================================
// Project: MonitoringAgent.Agent
// File: WindowsServiceHelper.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Provides helper methods for interacting with Windows services.
//
//      Includes functionality for retrieving runtime information about
//      Windows services that is not directly available through standard
//      service management APIs.
// ============================================================================

using System.Management;

namespace MonitoringAgent.Agent.Infrastructure;

/// <summary>
/// Provides helper methods for working with Windows services.
/// </summary>
public static class WindowsServiceHelper
{
    // =====================================================================
    // Service Information
    // =====================================================================

    /// <summary>
    /// Retrieves the process identifier (PID) associated with a Windows
    /// service.
    /// </summary>
    /// <param name="serviceName">
    /// Windows service name.
    /// </param>
    /// <returns>
    /// Process identifier if the service is running; otherwise null.
    /// </returns>
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