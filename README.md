# DotEukali.MetricsClient.Core
Simple Http wrapper for New Relic Metrics API.

To use, simply add configuration eg:

```
{
  "MetricsOptions": {
    "ApiKey": "{{your New Relic metrics api key}}",
    "ApiUrl": "{{this is optional, the current url is hard-coded}}",
    "Async": true|false, //(if excluded, this defaults to true, which is the existing default behaviour)
    "Attributes": {
      "host.name": "my.host",
      "app.name": "myapp",
      "myotherAttribute": 10
    }
  }
}
```
Change in 9.0.0:
- Removed support for <= net7.0
- added an overload for `AddMetricsClient` with an `IConfigurationSection` input.

If the attribute `app.name` exists, it will be prefixed to the metric name, eg `myapp_mymetricname`.
`ApiUrl` is optional, in case the default url needs to be overriden.
Attributes are parsed as `Dictionary<string, object>` and sent with every metric.

Microsoft dependency injection is used and can be wired up like:
```
serviceCollection.AddMetricsClient(Configuration); // will use "MetricsOptions" section
```
or
```
serviceCollection.AddMetricsClient(Configuration.GetSection("MyMetricsOptionsPath"));
```

`AddMetricsClient` returns `IHttpClientBuilder`, allowing Polly or other handlers to be applied.

Then, simply inject `IMetrics` where needed and use.
