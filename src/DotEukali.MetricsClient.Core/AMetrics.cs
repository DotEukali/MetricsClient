using System.Collections.Frozen;
using System.Collections.Generic;
using DotEukali.MetricsClient.Core.Infrastructure;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core;

internal abstract class AMetrics
{
    private readonly FrozenDictionary<string, object> _attributes;

    protected AMetrics(IOptions<MetricsOptions> options)
    {
        _attributes = (options?.Value?.Attributes ?? new Dictionary<string, object>()).ToFrozenDictionary();
    }

    protected Dictionary<string, object> BuildAttributes(KeyValuePair<string, object>[] attributes)
    {
        Dictionary<string, object> result = new();

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
