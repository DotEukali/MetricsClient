using System.ComponentModel;

namespace DotEukali.MetricsClient.Core.Models
{
    public enum MetricsType
    {   
        [Description("count")]
        Count,
        [Description("gauge")]
        Histogram
    }
}
