using DotEukali.MetricsClient.Core.HttpClients;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotEukali.MetricsClient.Core.Infrastructure.Startup
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds dependencies to the ServiceCollection and returns IHttpClientBuilder for metrics HttpClient to allow handlers like Polly to be applied.
        /// </summary>
        public static IHttpClientBuilder AddMetricsClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MetricsOptions>(configuration.GetSection(nameof(MetricsOptions)));

            serviceCollection.AddIMetrics(configuration);
            serviceCollection.AddTransient<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();

            return serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>();
        }
        
        /// <summary>
        /// Adds MetricsClient dependencies to the ServiceCollection and returns IServiceCollection to allow for fluent ServiceCollection statements.  Use <see cref="AddMetricsClient" /> instead if you want to configure HttpClient handlers.
        /// </summary>
        public static IServiceCollection RegisterMetrics(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MetricsOptions>(configuration.GetSection(nameof(MetricsOptions)));

            serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>();

            serviceCollection.AddIMetrics(configuration);
            serviceCollection.AddTransient<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();

            return serviceCollection;
        }

        private static IServiceCollection AddIMetrics(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            MetricsOptions options = new MetricsOptions();
            configuration.GetSection(nameof(MetricsOptions)).Bind(options);

            if (options.Async)
            {
                serviceCollection.AddSingleton<IMetrics, Metrics>();
            }
            else
            {
                serviceCollection.AddSingleton<IMetrics, SyncMetrics>();
            }

            return serviceCollection;
        }
    }
}
