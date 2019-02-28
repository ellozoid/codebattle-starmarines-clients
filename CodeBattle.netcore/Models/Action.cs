using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Action
    {
        [JsonProperty("from")]
        public int From { get; set; }

        [JsonProperty("to")]
        public int To { get; set; }

        [JsonProperty("unitsCount")]
        public int UnitsCount { get; set; }
    }
}