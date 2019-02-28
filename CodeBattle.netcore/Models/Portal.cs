using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Portal
    {
        [JsonProperty("source")]
        public int Source { get; set; }

        [JsonProperty("target")]
        public int Target { get; set; }
    }
}