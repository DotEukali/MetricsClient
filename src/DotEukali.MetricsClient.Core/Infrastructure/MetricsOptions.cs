using System.Collections.Generic;

namespace DotEukali.MetricsClient.Core.Infrastructure;

public sealed class MetricsOptions
{
    public string ApiKey { get; init; }
    public string ApiUrl { get; init; }
    public bool Async { get; init; } = true;
    public Dictionary<string, object> Attributes { get; init; } = [];
}
