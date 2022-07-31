using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DotEukali.MetricsClient.Core.Extensions;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotEukali.MetricsClient.Core.HttpClients
{
    internal class NewRelicClient : IMetricsClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<NewRelicClient> _logger;
        private readonly MetricsOptions _options;
        private readonly Uri _metricsUri;

        public NewRelicClient(HttpClient client, ILogger<NewRelicClient> logger, IOptions<MetricsOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
            _options = options.Value;

            if (!Uri.TryCreate(_options.ApiUrl, UriKind.Absolute, out _metricsUri))
            {
                _metricsUri = new Uri("https://metric-api.newrelic.com/metric/v1");
            }
        }

        public async Task SendMetricsAsync(MetricsItem metricsItem)
        {
            if (_metricsUri == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(metricsItem.Name))
            {
                throw new ArgumentNullException(nameof(metricsItem.Name));
            }

            if (!metricsItem.Attributes.ContainsKey("metric_value"))
            {
                metricsItem.Attributes.Add("metric_value", metricsItem.Value);
            }
            

            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _metricsUri))
            {
                requestMessage.Headers.Add("Api-Key", _options.ApiKey);
                requestMessage.Content = JsonContent.Create(metricsItem.ToMetricsPayload());

                using (HttpResponseMessage response = await _client.SendAsync(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger?.LogError($"{response.StatusCode}; {response.ReasonPhrase}; {await response.Content.ReadAsStringAsync()}");
                    }
                }
            }
        }

        public void SendMetrics(MetricsItem metricsItem)
        {
            if (_metricsUri == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(metricsItem.Name))
            {
                throw new ArgumentNullException(nameof(metricsItem.Name));
            }

            if (!metricsItem.Attributes.ContainsKey("metric_value"))
            {
                metricsItem.Attributes.Add("metric_value", metricsItem.Value);
            }

            using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _metricsUri))
            {
                requestMessage.Headers.Add("Api-Key", _options.ApiKey);
                requestMessage.Content = JsonContent.Create(metricsItem.ToMetricsPayload());

                using (HttpResponseMessage response = _client.Send(requestMessage))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        using (var reader = new StreamReader(response.Content.ReadAsStream()))
                        {
                            string responseText = reader.ReadToEnd();
                            _logger?.LogError($"{response.StatusCode}; {response.ReasonPhrase}; {responseText}");
                        }
                    }
                }
            }
        }
    }
}
