using System;
using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.MetricsImplementations;
using DotEukali.MetricsClient.Core.Models;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core;

internal sealed class SyncMetrics : AMetrics, IMetrics
{
    private readonly IMetricsClient _metricsClient;

    public SyncMetrics(IMetricsClient metricsClient, IOptions<MetricsOptions> options) : base(options)
    {
        _metricsClient = metricsClient;
    }

    public IDisposable Timer(string name, params KeyValuePair<string, object>[] attributes) => new SyncTimerMetric(_metricsClient, name, BuildAttributes(attributes));

    public void Count(string name, double value = 1D, params KeyValuePair<string, object>[] attributes) =>
        _metricsClient.SendMetrics(new MetricsItem(name, MetricsType.Count, value, BuildAttributes(attributes)));

    public void Histogram(string name, double value, params KeyValuePair<string, object>[] attributes) =>
        _metricsClient.SendMetrics(new MetricsItem(name, MetricsType.Histogram, value, BuildAttributes(attributes)));

}
