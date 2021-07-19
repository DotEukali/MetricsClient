using System.Collections.Generic;

namespace DotEukali.MetricsClient.Core.Models
{
    public class MetricsModel
    {
        public IEnumerable<MetricsItem> Metrics { get; set; }
    }
}
