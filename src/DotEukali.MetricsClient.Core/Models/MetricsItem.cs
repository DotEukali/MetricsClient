using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DotEukali.MetricsClient.Core.Extensions;

namespace DotEukali.MetricsClient.Core.Models
{
    public class MetricsItem
    {
        public MetricsItem(string name, MetricsType type, double value, IDictionary<string, object> attributes = null)
        {
            Type = type.Description();
            Value = value;
            Attributes = attributes;

            Name = BuildMetricName(name);
        }
        
        [JsonPropertyName("name")]
        public string Name { get; }
        
        [JsonPropertyName("type")]
        public string Type { get; }
        
        [JsonPropertyName("value")]
        public double Value { get; }

        [JsonPropertyName("timestamp")]
        public long Timestamp => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        [JsonPropertyName("interval.ms")]
        public int? IntervalMs => 60000;
        
        [JsonPropertyName("attributes")]
        public IDictionary<string, object> Attributes { get; set; }


        private string BuildMetricName(string name)
        {
            if (Attributes?.ContainsKey("app.name") == true)
            {
                return $"{Attributes["app.name"]}_{name}";
            }

            return name;
        }
    }
}
