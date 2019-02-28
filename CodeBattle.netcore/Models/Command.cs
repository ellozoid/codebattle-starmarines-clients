using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarMarines.Models
{
    public class Command
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("actions")]
        public IList<Action> Actions { get; set; }

        public Command()
        {
            Actions = new List<Action>();
        }
    }
}