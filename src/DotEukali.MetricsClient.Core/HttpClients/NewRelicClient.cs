using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DotEukali.MetricsClient.Core.Extensions;
using DotEukali.MetricsClient.Core.Infrastructure;
using DotEukali.MetricsClient.Core.Models;
using Microsoft.Extensions.Logging;

namespace DotEukali.MetricsClient.Core.HttpClients;

internal sealed class NewRelicClient : IMetricsClient
{
    private readonly HttpClient _client;
    private readonly ILogger<NewRelicClient> _logger;

    public NewRelicClient(HttpClient client, ILogger<NewRelicClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendMetricsAsync(MetricsItem metricsItem)
    {
        if (string.IsNullOrEmpty(metricsItem.Name))
        {
            throw new ArgumentNullException(nameof(metricsItem.Name));
        }

        if (!metricsItem.Attributes.ContainsKey("metric_value"))
        {
            metricsItem.Attributes.Add("metric_value", metricsItem.Value);
        }

        using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress);
        requestMessage.Content = JsonContent.Create(metricsItem.ToMetricsPayload());

        using HttpResponseMessage response = await _client.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            _logger?.LogError("{StatusCode}; {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
        }
    }

    public void SendMetrics(MetricsItem metricsItem)
    {
        try
        {
            if (string.IsNullOrEmpty(metricsItem.Name))
            {
                throw new ArgumentNullException(nameof(metricsItem.Name));
            }

            if (!metricsItem.Attributes.ContainsKey("metric_value"))
            {
                metricsItem.Attributes.Add("metric_value", metricsItem.Value);
            }

            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress);
            requestMessage.Content = JsonContent.Create(metricsItem.ToMetricsPayload());

            using HttpResponseMessage response = _client.Send(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogError("{StatusCode}; {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, ex.Message);
        }
    }
}
