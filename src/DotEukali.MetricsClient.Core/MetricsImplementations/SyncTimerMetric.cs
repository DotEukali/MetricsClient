using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.Models;

namespace DotEukali.MetricsClient.Core.MetricsImplementations;

internal sealed class SyncTimerMetric : IDisposable
{
    private readonly IMetricsClient _metricsClient;
    private readonly IDictionary<string, object> _attributes;
    private readonly Stopwatch _stopwatch;
    private readonly string _name;

    public SyncTimerMetric(IMetricsClient metricsClient, string name, IDictionary<string, object> attributes)
    {
        _metricsClient = metricsClient;
        _name = name;
        _attributes = attributes;

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    public void Dispose()
    {
        _stopwatch.Stop();

        _metricsClient.SendMetrics(new MetricsItem(_name, MetricsType.Histogram, _stopwatch.ElapsedMilliseconds, _attributes));
    }
}
