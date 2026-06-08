// ============================================================================
// Project: MonitoringAgent.Api
// File: SuppressAlertRequest.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Request model used to temporarily suppress an active alert.
//
//      Contains the user responsible for the suppression and the duration
//      that the alert should remain suppressed.
// ============================================================================

namespace MonitoringAgent.Api.Models.Requests;

/// <summary>
/// Represents a request to suppress an active alert.
/// </summary>
public sealed class SuppressAlertRequest
{
    /// <summary>
    /// Name of the user suppressing the alert.
    /// </summary>
    public string UserName
    {
        get;
        set;
    }
        = string.Empty;

    /// <summary>
    /// Number of hours the alert should remain suppressed.
    /// </summary>
    public double Hours
    {
        get;
        set;
    }
        = 24;
}