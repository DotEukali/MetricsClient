using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotEukali.MetricsClient.Core.Infrastructure.FireAndForget
{
    public class FireAndForgetMetricsHandler : IFireAndForgetMetricsHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FireAndForgetMetricsHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }
        
        public void Execute(Func<IMetricsClient, Task> metricsClient)
        {
            Task.Run(async () =>
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                ILogger<FireAndForgetMetricsHandler> logger = scope.ServiceProvider.GetService<ILogger<FireAndForgetMetricsHandler>>();

                try
                {
                    IMetricsClient client = scope.ServiceProvider.GetRequiredService<IMetricsClient>();
                    await metricsClient(client);
                }
                catch (Exception ex)
                {
                    logger?.LogError(ex, ex.Message);
                }
            });
        }
    }
}
