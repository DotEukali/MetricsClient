using System.ComponentModel;

namespace DotEukali.MetricsClient.Core.Models
{
    internal enum MetricsType
    {   
        [Description("count")]
        Count,
        [Description("gauge")]
        Histogram
    }
}
