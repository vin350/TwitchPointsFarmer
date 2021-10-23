using System;
using System.Collections.Generic;

namespace TwitchPointsFarmer.Models
{
    public class Command
    {
        public string Label { get; set; }
        public IEnumerable<Command> SubCommands { get; set; }
        public bool HasUserArgs { get; set; }
        public int NumberOfParameters { get; set; }
        public Action<object[]> Action { get; set; }
        public string Description { get; set; }
    }
}
