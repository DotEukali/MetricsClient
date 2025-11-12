using System;
using System.Threading;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.TestApp.Startup;
using Microsoft.Extensions.DependencyInjection;

namespace DotEukali.MetricsClient.TestApp;

public class MetricsClientApp
{
    private readonly IMetrics _metrics;

    public MetricsClientApp()
    {
        IServiceProvider serviceProvider = DependencyBuilder.GetServiceProvider();

        _metrics = serviceProvider.GetRequiredService<IMetrics>();
    }

    public bool SendTestMetrics(int sleepSeconds = 2, double count = 5, double histogram = 50)
    {
        using (_metrics.Timer("send_test_time"))
        {
            Thread.Sleep(TimeSpan.FromSeconds(sleepSeconds));

            _metrics.Count("send_test_count", count);
            _metrics.Histogram("send_test_histogram", histogram);
        }

        // give the using Dispose() time to make the http request before killing the process.
        Thread.Sleep(TimeSpan.FromSeconds(5));

        return true;
    }
}
