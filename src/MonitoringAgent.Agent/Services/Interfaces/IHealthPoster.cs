// ============================================================================
// Project : MonitoringAgent.Agent
// File    : IHealthPoster.cs
//
// Purpose
// -------
// Defines the contract used to publish health snapshots.
//
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Services.Interfaces;

/// <summary>
/// Publishes health snapshots.
/// </summary>
public interface IHealthPoster
{
    /// <summary>
    /// Publishes a health snapshot.
    /// </summary>
    Task PublishAsync(
        HealthSnapshot snapshot,
        CancellationToken cancellationToken);
}