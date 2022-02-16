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
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddMetricsClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MetricsOptions>(configuration.GetSection(nameof(MetricsOptions)));

            serviceCollection.AddSingleton<IMetrics, Metrics>();
            serviceCollection.AddTransient<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();

            return serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>();
        }


        /// <summary>
        /// Adds MetricsClient dependencies to the ServiceCollection and returns IServiceCollection to allow for fluent ServiceCollection statements.  Use AddMetricsClient instead if you want to configure HttpClient handlers.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterMetrics(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MetricsOptions>(configuration.GetSection(nameof(MetricsOptions)));

            serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>();

            serviceCollection.AddSingleton<IMetrics, Metrics>();
            serviceCollection.AddTransient<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();

            return serviceCollection;
        }
    }
}
