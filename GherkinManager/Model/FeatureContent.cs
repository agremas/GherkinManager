using System.Text.Json.Serialization;

namespace GherkinManager.Model
{
    public class FeatureContent
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }
}
