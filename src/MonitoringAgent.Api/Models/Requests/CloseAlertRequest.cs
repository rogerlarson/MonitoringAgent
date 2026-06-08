// ============================================================================
// Project: MonitoringAgent.Api
// File: CloseAlertRequest.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Request model used when manually closing an active alert.
//
//      Contains the user responsible for closing the alert.
// ============================================================================

namespace MonitoringAgent.Api.Models.Requests;

/// <summary>
/// Represents a request to manually close an active alert.
/// </summary>
public sealed class CloseAlertRequest
{
    /// <summary>
    /// Name of the user closing the alert.
    /// </summary>
    public string UserName
    {
        get;
        set;
    }
        = string.Empty;
}