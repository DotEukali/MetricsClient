using System.Collections.Generic;

namespace DotEukali.MetricsClient.Core.Infrastructure
{
    public class MetricsOptions
    {
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public IDictionary<string, object> Attributes { get; set; }
    }
}
