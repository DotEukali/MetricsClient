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
Change in 7.0.0, have targeted net6 and net7 specifically, instead of netstandard2.1. 

Added with 6.1.0 is synchronous metrics sending - not usually desired, but I have added it for use where the background process might get prematurely terminated.  This is configured in appsettings and applies to everything, maybe I'll update it later so the send method can be chosen as needed in the code...

If the attribute `app.name` exists, it will be prefixed to the metric name, eg `myapp_mymetricname`.
`ApiUrl` is optional, in case the default url needs to be overriden.
Attributes are parsed as `IDictionary<string, object>` and sent with every metric.

Polly has been removed as a dependency as of 0.2.3 and a new registration extension created to replace the old which returns IHttpClientBuilder, allowing Polly or other handling logic to be added explicitly.

Microsoft dependency injection is used and can be wired up like:
```
serviceCollection.AddMetricsClient(Configuration);
```

Then, simply inject `IMetrics` where needed and use.
