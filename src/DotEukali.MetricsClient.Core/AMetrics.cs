using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Infrastructure;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core
{
    internal abstract class AMetrics
    {
        private readonly IDictionary<string, object> _attributes;

        protected AMetrics(IOptions<MetricsOptions> options)
        {
            _attributes = options?.Value?.Attributes ?? new Dictionary<string, object>();
        }

        protected IDictionary<string, object> BuildAttributes(KeyValuePair<string, object>[] attributes)
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
