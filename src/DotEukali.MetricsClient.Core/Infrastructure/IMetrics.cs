using System;
using System.Collections.Generic;

namespace DotEukali.MetricsClient.Core.Infrastructure;

public interface IMetrics
{
    IDisposable Timer(string name, params KeyValuePair<string, object>[] attributes);
    void Count(string name, double value = 1D, params KeyValuePair<string, object>[] attributes);
    void Histogram(string name, double value, params KeyValuePair<string, object>[] attributes);
}
