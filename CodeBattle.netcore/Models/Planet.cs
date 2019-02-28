using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Planet
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("droids")]
        public int Droids { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("neighbours")]
        public int[] Neighbours { get; set; }
    }
}