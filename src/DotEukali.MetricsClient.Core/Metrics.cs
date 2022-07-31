using System;
using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using DotEukali.MetricsClient.Core.MetricsImplementations;
using DotEukali.MetricsClient.Core.Models;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core
{
    internal class Metrics : AMetrics, IMetrics
    {
        private readonly IFireAndForgetMetricsHandler _fireAndForgetMetrics;

        public Metrics(IFireAndForgetMetricsHandler fireAndForgetMetrics, IOptions<MetricsOptions> options) : base(options)
        {
            _fireAndForgetMetrics = fireAndForgetMetrics ?? throw new ArgumentNullException(nameof(fireAndForgetMetrics));
        }

        public IDisposable Timer(string name, params KeyValuePair<string, object>[] attributes) => new TimerMetric(_fireAndForgetMetrics, name, BuildAttributes(attributes));

        public void Count(string name, double value = 1D, params KeyValuePair<string, object>[] attributes)
            => _fireAndForgetMetrics.Execute(async metricsClient =>
            {
                await metricsClient.SendMetricsAsync(new MetricsItem(name, MetricsType.Count, value, BuildAttributes(attributes)));
            });

        public void Histogram(string name, double value, params KeyValuePair<string, object>[] attributes)
            => _fireAndForgetMetrics.Execute(async metricsClient =>
            {
                await metricsClient.SendMetricsAsync(new MetricsItem(name, MetricsType.Histogram, value, BuildAttributes(attributes)));
            });
    }
}
