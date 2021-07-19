using System.Threading.Tasks;
using DotEukali.MetricsClient.Core.Models;

namespace DotEukali.MetricsClient.Core.Infrastructure
{
    public interface IMetricsClient
    {
        Task SendMetricsAsync(MetricsItem metricsItem);
    }
}
