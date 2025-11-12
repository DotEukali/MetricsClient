using DotEukali.MetricsClient.TestApp;
using Xunit;

namespace DotEukali.MetricsClient.Tests;

public class RunMetricsApp
{
    [Fact]
    public void SendTestMetrics()
    {
        MetricsClientApp app = new MetricsClientApp();

        Assert.True(app.SendTestMetrics());
    }
}
