using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Disaster
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sourcePlanetId")]
        public int SourcePlanetId { get; set; }

        [JsonProperty("TargetPlanetId")]
        public int TargetPlanetId { get; set; }

        [JsonProperty("planetId")]
        public int PlanetId { get; set; }
    }
}