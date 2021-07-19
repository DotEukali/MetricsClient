using System;
using DotEukali.MetricsClient.Core.Infrastructure.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotEukali.MetricsClient.TestApp.Startup
{
    public class DependencyBuilder
    {
        private IServiceProvider _serviceProvider;

        public IServiceProvider GetServiceProvider() => _serviceProvider 
            ??= new ServiceCollection()
                .RegisterMetrics(GetConfiguration())
                .BuildServiceProvider();

        private IConfiguration GetConfiguration()
        {
            ConfigurationBuilder config = new ConfigurationBuilder();
            config.AddJsonFile("C:\\dev\\DotEukali.MetricsClient.TestApp\\appconfig.json", false);

            return config.Build();
        }
    }
}
