using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Screen
    {
        [JsonProperty("planets")]
        public Planet[] Planets { get; set; }

        [JsonProperty("disaster")]
        public Disaster[] Disasters { get; set; }

        [JsonProperty("portals")]
        public Portal[] Portals { get; set; }

        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}