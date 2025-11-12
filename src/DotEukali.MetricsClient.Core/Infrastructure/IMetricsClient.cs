using System.Threading.Tasks;
using DotEukali.MetricsClient.Core.Models;

namespace DotEukali.MetricsClient.Core.Infrastructure;

internal interface IMetricsClient
{
    Task SendMetricsAsync(MetricsItem metricsItem);
    void SendMetrics(MetricsItem metricsItem);
}
