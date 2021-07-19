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

Microsoft dependency injection is used and can be wired up like:
```
serviceCollection.RegisterMetrics(Configuration);
```

Then, simply inject `IMetrics` where needed and use.
