using System;
using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.Infrastructure.FireAndForget;
using DotEukali.MetricsClient.Core.MetricsImplementations;
using DotEukali.MetricsClient.Core.Models;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core
{
    public class Metrics : IMetrics
    {
        private readonly IFireAndForgetMetricsHandler _fireAndForgetMetrics;
        private readonly IDictionary<string, object> _attributes;

        public Metrics(IFireAndForgetMetricsHandler fireAndForgetMetrics, IOptions<MetricsOptions> options)
        {
            _fireAndForgetMetrics = fireAndForgetMetrics ?? throw new ArgumentNullException(nameof(fireAndForgetMetrics));
            _attributes = options?.Value?.Attributes ?? new Dictionary<string, object>();
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

        private IDictionary<string, object> BuildAttributes(KeyValuePair<string, object>[] attributes)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            if (attributes != null)
            {
                foreach (var item in attributes)
                {
                    result.Add(item.Key, item.Value);
                }
            }

            foreach (var item in _attributes)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
    }
}
