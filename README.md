# DotEukali.MetricsClient.Core
Simple Http wrapper for New Relic Metrics API, using DotNet Core 5.

To use, simply add configuration eg:

```
{
  "MetricsOptions": {
    "ApiKey": "{{your New Relic metrics api key}}",
    "ApiUrl": "{{this is optional, the current url is hard-coded}}",
    "Attributes": {
      "host.name": "my.host",
      "app.name": "myapp",
      "myotherAttribute": 10
    }
  }
}
```

If the attribute `app.name` exists, it will be prefixed to the metric name, eg `myapp_mymetricname`.
`ApiUrl` is optional, in case the default url needs to be overriden.
Attributes are parsed as `IDictionary<string, object>` and sent with every metric.

Polly has been removed as a dependency as of 0.2.3 and a new registration extension created to replace the old which returns IHttpClientBuilder, allowing Polly or other handling logic to be added explicitly.

Microsoft dependency injection is used and can be wired up like:
```
serviceCollection.AddMetricsClient(Configuration);
```

Then, simply inject `IMetrics` where needed and use.
