using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Models;

namespace DotEukali.MetricsClient.Core.Extensions
{
    public static class MetricsItemExtensions
    {
        public static IEnumerable<MetricsModel> ToMetricsPayload(this MetricsItem metricsItem) => 
            new[]
            {
                new MetricsModel()
                {
                    Metrics = new[]
                    {
                        metricsItem
                    }
                }
            };
    }
}
