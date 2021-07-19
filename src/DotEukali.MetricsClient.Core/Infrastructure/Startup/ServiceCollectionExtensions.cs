using System;
using DotEukali.MetricsClient.Core.HttpClients;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DotEukali.MetricsClient.Core.Infrastructure.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMetrics(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MetricsOptions>(configuration.GetSection(nameof(MetricsOptions)));
         
            serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>()
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(4),
                        TimeSpan.FromSeconds(8)
                    }
                ));

            serviceCollection.AddSingleton<IMetrics, Metrics>();
            serviceCollection.AddTransient<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();

            return serviceCollection;
        }
    }
}
