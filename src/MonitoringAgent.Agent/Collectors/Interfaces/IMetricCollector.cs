using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringAgent.Common.Models;

namespace MonitoringAgent.Agent.Collectors.Interfaces;

/// <summary>
/// Collects health metrics from the local machine.
/// </summary>
public interface IMetricCollector
{
    /// <summary>
    /// Collects a health snapshot.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// A populated health snapshot.
    /// </returns>
    Task<HealthSnapshot> CollectAsync(
        CancellationToken cancellationToken);
}
