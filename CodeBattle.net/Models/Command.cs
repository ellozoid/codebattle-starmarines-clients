using System;
using System.Collections.Generic;

namespace StarMarines.Models
{
    public class Command {
        public string token;
        public IList<Action> actions = new List<Action>();
    }
}