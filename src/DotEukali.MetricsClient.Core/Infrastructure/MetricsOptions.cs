using System.Collections.Generic;

namespace DotEukali.MetricsClient.Core.Infrastructure
{
    internal class MetricsOptions
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public bool Async { get; set; } = true;
        public IDictionary<string, object> Attributes { get; set; }
    }
}
