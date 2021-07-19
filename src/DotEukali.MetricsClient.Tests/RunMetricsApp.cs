using System;
using System.Threading.Tasks;
using DotEukali.MetricsClient.TestApp;
using FluentAssertions;
using Xunit;

namespace DotEukali.MetricsClient.Tests
{
    public class RunMetricsApp
    {
        [Fact]
        public void SendTestMetrics()
        {
            MetricsClientApp app = new MetricsClientApp();
            
            Func<Task<bool>> act = async () => await app.SendTestMetricsAsync();

            act.Invoke().Result.Should().Be(true);
        }
    }
}
