// ============================================================================
// Project: MonitoringAgent
// File: EmailSettings.cs
// Author: Roger Larson
// Date Created: 06/07/2026
// Date Updated: 06/07/2026
// Description:
//      Defines SMTP configuration settings used for email notification
//      delivery.
//
//      These settings control SMTP server connectivity, authentication,
//      sender and recipient addresses, and SSL configuration for alert
//      notification emails.
// ============================================================================

namespace MonitoringAgent.Common.Configuration;

/// <summary>
/// Defines SMTP configuration settings used for email delivery.
/// </summary>
public sealed class EmailSettings
{
    // =====================================================================
    // SMTP Server
    // =====================================================================

    /// <summary>
    /// SMTP server host name or IP address.
    /// </summary>
    public string Host
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// SMTP server port.
    /// </summary>
    public int Port
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates whether SSL should be used when connecting to the SMTP
    /// server.
    /// </summary>
    public bool EnableSsl
    {
        get;
        set;
    } = true;

    // =====================================================================
    // Authentication
    // =====================================================================

    /// <summary>
    /// SMTP account user name.
    /// </summary>
    public string UserName
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// SMTP account password.
    /// </summary>
    public string Password
    {
        get;
        set;
    } = string.Empty;

    // =====================================================================
    // Email Addresses
    // =====================================================================

    /// <summary>
    /// Sender email address used for outgoing notifications.
    /// </summary>
    public string FromAddress
    {
        get;
        set;
    } = string.Empty;

    /// <summary>
    /// Recipient email address for alert notifications.
    /// </summary>
    public string ToAddress
    {
        get;
        set;
    } = string.Empty;
}