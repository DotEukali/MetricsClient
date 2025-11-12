using System;
using DotEukali.MetricsClient.Core.HttpClients;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotEukali.MetricsClient.Core.Infrastructure.Startup;

public static class ServiceCollectionExtensions
{
    private const string DefaultApiUrl = "https://metric-api.newrelic.com/metric/v1";

    /// <summary>
    /// Adds dependencies to the ServiceCollection and returns IHttpClientBuilder for metrics HttpClient to allow handlers like Polly to be applied.
    /// </summary>
    public static IHttpClientBuilder AddMetricsClient(this IServiceCollection serviceCollection, IConfiguration configuration) =>
        AddMetricsClient(serviceCollection, configuration.GetSection(nameof(MetricsOptions)));

    /// <summary>
    /// Adds dependencies to the ServiceCollection and returns IHttpClientBuilder for metrics HttpClient to allow handlers like Polly to be applied.
    /// </summary>
    public static IHttpClientBuilder AddMetricsClient(this IServiceCollection serviceCollection, IConfigurationSection metricsConfigSection)
    {
        serviceCollection.Configure<MetricsOptions>(metricsConfigSection);

        MetricsOptions metricsOptions = metricsConfigSection.Get<MetricsOptions>();

        serviceCollection.AddIMetrics(metricsOptions);

        return serviceCollection.AddHttpClient<IMetricsClient, NewRelicClient>(options =>
        {
            options.BaseAddress =
                Uri.TryCreate(metricsOptions.ApiUrl, UriKind.Absolute, out Uri metricsUri)
                    ? metricsUri
                    : new Uri(DefaultApiUrl);

            options.DefaultRequestHeaders.Add("Api-Key", metricsOptions.ApiKey);
        });
    }

    private static IServiceCollection AddIMetrics(this IServiceCollection serviceCollection, MetricsOptions options)
    {
        if (options.Async)
        {
            serviceCollection.AddSingleton<IFireAndForgetMetricsHandler, FireAndForgetMetricsHandler>();
            serviceCollection.AddSingleton<IMetrics, Metrics>();
        }
        else
        {
            serviceCollection.AddSingleton<IMetrics, SyncMetrics>();
        }

        return serviceCollection;
    }
}
