using System;
using System.Threading.Tasks;

namespace DotEukali.MetricsClient.Core.Infrastructure.FireAndForget
{
    public interface IFireAndForgetMetricsHandler
    {
        void Execute(Func<IMetricsClient, Task> metricsClient);
    }
}
