using System;
using DotEukali.MetricsClient.Core.Infrastructure.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotEukali.MetricsClient.TestApp.Startup;

public static class DependencyBuilder
{
    private static IServiceProvider _serviceProvider;

    public static IServiceProvider GetServiceProvider()
    {
        if (_serviceProvider != null)
            return _serviceProvider;

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddMetricsClient(GetConfiguration().GetSection("MetricsOptions"));

        _serviceProvider = serviceCollection.BuildServiceProvider();

        return _serviceProvider;
    }

    private static IConfiguration GetConfiguration()
    {
        ConfigurationBuilder config = new ConfigurationBuilder();
        config.AddUserSecrets<MetricsClientApp>();

        return config.Build();
    }
}
