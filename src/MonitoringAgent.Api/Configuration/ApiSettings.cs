// ============================================================================
// Project: MonitoringAgent.Api
// File: ApiSettings.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines API authentication settings used by the monitoring API.
//
//      These settings control whether API key authentication is required
//      and specify the API key used to authorize monitoring agents and
//      other API clients.
// ============================================================================

namespace MonitoringAgent.Api.Configuration;

/// <summary>
/// Defines API authentication settings.
/// </summary>
public sealed class ApiSettings
{
    // =====================================================================
    // Authentication
    // =====================================================================

    /// <summary>
    /// Indicates whether API key authentication is required.
    /// </summary>
    public bool RequireApiKey
    {
        get;
        set;
    }

    /// <summary>
    /// API key used to authenticate requests.
    /// </summary>
    public string ApiKey
    {
        get;
        set;
    } = string.Empty;
}