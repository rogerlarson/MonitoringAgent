// ============================================================================
// Project: MonitoringAgent.Api
// File: AcknowledgeAlertRequest.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Request model used when acknowledging an active alert.
//
//      Contains the user responsible for acknowledging the alert.
// ============================================================================

namespace MonitoringAgent.Api.Models.Requests;

/// <summary>
/// Represents a request to acknowledge an active alert.
/// </summary>
public sealed class AcknowledgeAlertRequest
{
    /// <summary>
    /// Name of the user acknowledging the alert.
    /// </summary>
    public string UserName
    {
        get;
        set;
    }
        = string.Empty;
}