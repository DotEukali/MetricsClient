using System;
using System.Collections.Generic;
using System.Diagnostics;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using DotEukali.MetricsClient.Core.Models;

namespace DotEukali.MetricsClient.Core.MetricsImplementations
{
    public class TimerMetric : IDisposable
    {
        private readonly IFireAndForgetMetricsHandler _fireAndForgetMetrics;
        private readonly IDictionary<string, object> _attributes;
        private readonly Stopwatch _stopwatch;
        private readonly string _name;

        public TimerMetric(IFireAndForgetMetricsHandler fireAndForgetMetrics, string name, IDictionary<string, object> attributes)
        {
            _fireAndForgetMetrics = fireAndForgetMetrics ?? throw new ArgumentNullException(nameof(fireAndForgetMetrics));
            _name = name;
            _attributes = attributes;

            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            _fireAndForgetMetrics.Execute(async metricsClient =>
            {
                await metricsClient.SendMetricsAsync(new MetricsItem(_name, MetricsType.Histogram, _stopwatch.ElapsedMilliseconds, _attributes));
            });
        }
    }
}
